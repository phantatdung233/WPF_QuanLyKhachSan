const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

// Helper executing gh CLI safely without /bin/sh interpolation
function gh(args, options = {}) {
  return spawnSync('gh', args, {
    encoding: 'utf8',
    env: { ...process.env, ...options.env },
    stdio: options.stdio || ['pipe', 'pipe', 'pipe'],
  });
}

function run() {
  console.log('=====================================================');
  console.log('IT008 - SYNC TASKS TO GITHUB MILESTONES, ISSUES & DEPS');
  console.log('=====================================================');

  const repo = process.env.GITHUB_REPOSITORY;
  if (!repo) {
    console.error('Missing GITHUB_REPOSITORY (e.g. phantatdung233/WPF_QuanLyKhachSan)');
    process.exit(1);
  }

  const memberA = (process.env.MEMBER_A_GITHUB || '').trim();
  const memberB = (process.env.MEMBER_B_GITHUB || '').trim();
  const memberC = (process.env.MEMBER_C_GITHUB || '').trim();

  // 1. TẠO HOẶC CẬP NHẬT 3 MILESTONES (CÓ HẠN CHÓT 18/11 -> 30/12/2026)
  console.log('\n1. Kiểm tra & Đồng bộ Milestones...');
  const milestoneConfigs = [
    {
      prefix: 'Mốc 1:',
      title: 'Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (18/11 – 04/12)',
      dueOn: '2026-12-04T08:00:00Z',
      description: 'Sprint 0 mở khóa nền (TASK-01, TASK-02 xong trước 23/11) + Đăng nhập phân quyền & 3 danh mục cơ bản (TASK-03, TASK-04, TASK-05, TASK-06 xong trước 04/12).'
    },
    {
      prefix: 'Mốc 2:',
      title: 'Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (05/12 – 18/12)',
      dueOn: '2026-12-18T08:00:00Z',
      description: 'Sơ đồ phòng DataTrigger (TASK-09 khởi động sớm từ 30/11), Đặt phòng chống trùng lịch (TASK-07), Check-in/Check-out & Dịch vụ phòng (TASK-08), Hóa đơn & Xuất file .txt (TASK-10).'
    },
    {
      prefix: 'Mốc 3:',
      title: 'Mốc 3: Báo Cáo Thống Kê & Chốt Demo (19/12 – 30/12)',
      dueOn: '2026-12-30T08:00:00Z',
      description: 'Báo cáo doanh thu LiveCharts2 (TASK-11, Feature Freeze 24/12) + Kiểm thử E2E, Slide & 2 buổi Rehearsal (TASK-12, chốt hạn 30/12).'
    }
  ];

  let existingMilestones = [];
  const msRes = gh(['api', `repos/${repo}/milestones?state=all&per_page=100`]);
  if (msRes.status === 0 && msRes.stdout) {
    try {
      existingMilestones = JSON.parse(msRes.stdout);
    } catch (e) {
      console.warn('Không thể parse danh sách milestones:', e.message);
    }
  }

  for (const cfg of milestoneConfigs) {
    const found = existingMilestones.find(m => m.title === cfg.title || m.title.startsWith(cfg.prefix));
    if (found) {
      console.log(`  ~ Cập nhật Milestone #${found.number}: "${cfg.title}" (due ${cfg.dueOn})`);
      gh([
        'api', '--method', 'PATCH', `repos/${repo}/milestones/${found.number}`,
        '-f', `title=${cfg.title}`,
        '-f', `due_on=${cfg.dueOn}`,
        '-f', `description=${cfg.description}`,
        '-f', 'state=open'
      ]);
    } else {
      console.log(`  + Tạo Milestone mới: "${cfg.title}"...`);
      gh([
        'api', `repos/${repo}/milestones`,
        '-f', `title=${cfg.title}`,
        '-f', `due_on=${cfg.dueOn}`,
        '-f', `description=${cfg.description}`
      ]);
    }
  }

  // 2. KHỞI TẠO BỘ LABELS MÀU CHUẨN
  console.log('\n2. Kiểm tra & Khởi tạo Labels màu...');
  const labelColorMap = {
    'ready-for-agent': '0e8a16',
    'core': '0052cc',
    'architecture': '1d76db',
    'database': '0e8a16',
    'navigation': '5319e7',
    'auth': 'd93f0b',
    'security': 'b60205',
    'room': 'fbca04',
    'crud': '0052cc',
    'customer': '1f883d',
    'service': 'bfd4f2',
    'booking': 'e99695',
    'business-logic': 'd4c5f9',
    'workflow': 'c5def5',
    'invoice': '0e8a16',
    'billing': '2cbe4e',
    'ui': 'f9d0c4',
    'theme': 'e36209',
    'layout': 'cfd3d7',
    'advanced': 'b60205',
    'room-map': 'd93f0b',
    'stats': '6f42c1',
    'report': '6f42c1',
    'chart': '5319e7',
    'qa': 'd93f0b',
    'docs': '0075ca',
    'presentation': 'a2eeef',
    'demo': '7057ff',
    'priority-high': 'b60205',
    'priority-medium': 'fbca04',
    'priority-low': '0e8a16',
    'm1': '1d76db',
    'm2': '5319e7',
    'm3': '6f42c1'
  };

  for (const [name, color] of Object.entries(labelColorMap)) {
    gh(['label', 'create', name, '--color', color, '--force']);
  }
  console.log('  OK: Đã cập nhật bộ nhãn màu chuẩn.');

  // 3. PARSE docs/tasks/*.md
  console.log('\n3. Phân tích cú pháp docs/tasks/*.md...');
  const tasksDir = path.join(process.cwd(), 'docs', 'tasks');
  if (!fs.existsSync(tasksDir)) {
    console.error('Không tìm thấy thư mục docs/tasks/!');
    process.exit(1);
  }

  const taskFiles = fs.readdirSync(tasksDir)
    .filter(f => /^TASK-\d+\.md$/.test(f))
    .sort();

  if (taskFiles.length === 0) {
    console.error('Không tìm thấy file docs/tasks/TASK-*.md nào!');
    process.exit(1);
  }

  const tasks = [];

  for (const fileName of taskFiles) {
    const content = fs.readFileSync(path.join(tasksDir, fileName), 'utf8');
    const lines = content.split(/\r?\n/);

    const headerLine = lines.find(l => /^#\s+\[TASK-\d+\]/.test(l));
    if (!headerLine) {
      console.warn(`  [WARN] Bỏ qua ${fileName}: không có heading "# [TASK-xx]".`);
      continue;
    }
    const headerMatch = headerLine.match(/^#\s+\[(TASK-\d+)\]\s+(.+)$/);

    const getMeta = (key) => {
      const ln = lines.find(l => l.startsWith(`**${key}**`));
      return ln ? ln.split(':').slice(1).join(':').trim() : '';
    };

    const sanitize = (v) => (v || '').replace(/[`*"']/g, '').trim();

    const labels = (getMeta('Labels') || '')
      .split(',')
      .map(l => l.replace(/[`*\s]/g, ''))
      .filter(Boolean);

    if (!labels.includes('ready-for-agent')) {
      labels.push('ready-for-agent');
    }

    const blockedRaw = getMeta('Blocked by');
    const blockerIssueNumbers = Array.from(blockedRaw.matchAll(/#(\d+)/g), m => Number(m[1]));

    const parentSpecRaw = getMeta('Part of Spec');
    const parentMatch = parentSpecRaw.match(/#(\d+)/);
    const parentSpecNumber = parentMatch ? Number(parentMatch[1]) : 77;

    tasks.push({
      id: headerMatch[1].trim(),
      title: `[${headerMatch[1].trim()}] ${headerMatch[2].trim()}`,
      assignee: getMeta('Thành viên đảm nhiệm').replace(/\*\*/g, '').trim(),
      priority: sanitize(getMeta('Mức độ ưu tiên')) || 'Medium',
      labels,
      branch: sanitize(getMeta('Nhánh làm việc')),
      milestone: getMeta('Milestone'),
      schedule: getMeta('Khung thời gian dự kiến'),
      blockedRaw,
      blockerIssueNumbers,
      parentSpecNumber
    });
  }

  console.log(`  OK: Đã trích xuất ${tasks.length} tasks từ docs/tasks/.`);

  // 4. LẤY DANH SÁCH ISSUES HIỆN CÓ
  console.log('\n4. Đồng bộ GitHub Issues...');
  let existingIssues = [];
  const listRes = gh(['issue', 'list', '--state', 'all', '--limit', '200', '--json', 'number,title,state']);
  if (listRes.status === 0 && listRes.stdout) {
    try {
      existingIssues = JSON.parse(listRes.stdout);
    } catch (e) {
      console.warn('Không thể parse danh sách issues:', e.message);
    }
  }

  function buildBody(task) {
    const detailPath = path.join(process.cwd(), 'docs', 'tasks', `${task.id}.md`);
    const raw = fs.readFileSync(detailPath, 'utf8');
    const rawLines = raw.split(/\r?\n/);
    const getMeta = (key) => {
      const ln = rawLines.find(l => l.startsWith(`**${key}**`));
      return ln ? ln.split(':').slice(1).join(':').trim() : '';
    };
    const member = getMeta('Thành viên đảm nhiệm');
    const branch = getMeta('Nhánh làm việc');
    const blocked = getMeta('Blocked by');
    const schedule = getMeta('Khung thời gian dự kiến');
    const startIdx = rawLines.findIndex(l => l.startsWith('## 1.'));
    const sections = startIdx !== -1
      ? rawLines.slice(startIdx).filter(l => !l.startsWith('*Tài liệu tự động')).join('\n').replace(/\n+$/, '')
      : '';
    const labelsStr = task.labels.map(l => `\`${l}\``).join(', ');
    const issueLine = getMeta('GitHub Issue');
    const issueNumMatch = issueLine.match(/#(\d+)/);
    const issueNum = issueNumMatch ? issueNumMatch[1] : '';

    const rows = [
      `| Mã Task | \`${task.id}\` |`,
      issueNum ? `| GitHub Issue | #${issueNum} |` : `| GitHub Issue | — |`,
      `| Thành viên | ${member} |`,
      `| Ưu tiên | ${task.priority} |`,
      `| Branch | ${branch} |`,
      `| Milestone | ${task.milestone} |`,
      schedule ? `| Khung thời gian | ${schedule} |` : null,
      `| Blocked by | ${blocked} |`,
      `| Labels | ${labelsStr} |`
    ].filter(Boolean);

    return [
      `> Part of SPEC #${task.parentSpecNumber} · Docs: \`docs/tasks/${task.id}.md\` · Spec: \`docs/specs/hotel-management-system.md\` · Design System: \`DESIGN.md\``,
      '',
      `# ${task.title}`,
      '',
      '## Thông tin Task',
      '',
      '| Trường | Giá trị |',
      '|---|---|',
      ...rows,
      '',
      '---',
      '',
      sections,
      '',
      '---',
      '',
      '## Tham khảo',
      '',
      `- SPEC tổng: #${task.parentSpecNumber} — Hotel Management System (WPF .NET 10 MVVM, SQL Server + ADO.NET)`,
      `- Spec tổng: [\`docs/specs/hotel-management-system.md\`](https://github.com/${repo}/blob/main/docs/specs/hotel-management-system.md)`,
      `- Hệ thống thiết kế & Tokens: [\`DESIGN.md\`](https://github.com/${repo}/blob/main/DESIGN.md)`,
      `- Quy tắc Agent 2 Pha: [\`CLAUDE.md\`](https://github.com/${repo}/blob/main/CLAUDE.md)`,
      `- Tài liệu gốc: [\`docs/tasks/${task.id}.md\`](https://github.com/${repo}/blob/main/docs/tasks/${task.id}.md)`,
      `- Glossary thuật ngữ: [\`GLOSSARY.md\`](https://github.com/${repo}/blob/main/GLOSSARY.md)`,
      '',
      `*Đồng bộ từ \`${task.id}.md\` — mọi chỉnh sửa thực hiện trong \`docs/tasks/${task.id}.md\` rồi chạy workflow sync để cập nhật Issue${issueNum ? ` #${issueNum}` : ''}.*`,
      ''
    ].join('\n');
  }

  const syncedTaskIssues = [];

  for (const task of tasks) {
    let assigneeUsername = '';
    if (task.assignee.includes('Member A') && memberA) assigneeUsername = memberA;
    else if (task.assignee.includes('Member B') && memberB) assigneeUsername = memberB;
    else if (task.assignee.includes('Member C') && memberC) assigneeUsername = memberC;

    const body = buildBody(task);
    const labels = [...new Set([...task.labels, `priority-${task.priority.toLowerCase()}`])];

    const existing = existingIssues.find(i => i.title.startsWith(`[${task.id}]`));

    if (existing) {
      console.log(`  ~ Cập nhật Issue #${existing.number}: ${task.title}`);
      const editArgs = ['issue', 'edit', String(existing.number), '--body', body, '--title', task.title];
      if (task.milestone) {
        editArgs.push('--milestone', task.milestone);
      }
      for (const lbl of labels) {
        editArgs.push('--add-label', lbl);
      }
      if (assigneeUsername) {
        editArgs.push('--add-assignee', assigneeUsername);
      }

      let res = gh(editArgs);
      if (res.status !== 0 && assigneeUsername && (res.stderr.includes('assignee') || res.stderr.includes('Could not resolve to a User'))) {
        const fallbackEditArgs = editArgs.filter((a, idx) => a !== '--add-assignee' && editArgs[idx - 1] !== '--add-assignee');
        res = gh(fallbackEditArgs);
      }
      if (res.status === 0) {
        syncedTaskIssues.push({ ...task, number: existing.number });
      } else {
        console.error(`    [ERR] Lỗi cập nhật #${existing.number}:`, res.stderr);
      }
    } else {
      console.log(`  + Tạo mới Issue: ${task.title}`);
      const createArgs = ['issue', 'create', '--title', task.title, '--body', body];
      if (task.milestone) {
        createArgs.push('--milestone', task.milestone);
      }
      for (const lbl of labels) {
        createArgs.push('--label', lbl);
      }
      if (assigneeUsername) {
        createArgs.push('--assignee', assigneeUsername);
      }

      let res = gh(createArgs);
      if (res.status !== 0 && assigneeUsername && (res.stderr.includes('assignee') || res.stderr.includes('Could not resolve to a User'))) {
        const fallbackCreateArgs = createArgs.filter((a, idx) => a !== '--assignee' && createArgs[idx - 1] !== '--assignee');
        res = gh(fallbackCreateArgs);
      }
      if (res.status === 0) {
        const createdUrl = res.stdout.trim();
        const numMatch = createdUrl.match(/\/issues\/(\d+)$/);
        if (numMatch) {
          syncedTaskIssues.push({ ...task, number: Number(numMatch[1]) });
        }
      } else {
        console.error(`    [ERR] Lỗi tạo issue:`, res.stderr);
      }
    }
  }

  // 5. ĐỒNG BỘ SUB-ISSUES VÀ ĐỒ THỊ NATIVE BLOCKED_BY TRÊN GITHUB
  console.log('\n5. Đồng bộ Sub-issues & Native Issue Dependencies (blocked_by)...');
  const issueDbIdByNumber = new Map();
  for (const item of syncedTaskIssues) {
    const idRes = gh(['api', `repos/${repo}/issues/${item.number}`, '--jq', '.id']);
    if (idRes.status === 0 && idRes.stdout.trim()) {
      issueDbIdByNumber.set(item.number, Number(idRes.stdout.trim()));
    }
  }

  for (const item of syncedTaskIssues) {
    const childDbId = issueDbIdByNumber.get(item.number);
    if (childDbId && item.parentSpecNumber) {
      gh(['api', '--method', 'POST', `repos/${repo}/issues/${item.parentSpecNumber}/sub_issues`, '-F', `sub_issue_id=${childDbId}`]);
    }

    // Lấy danh sách native blocked_by hiện tại của issue
    const currentRes = gh(['api', `repos/${repo}/issues/${item.number}/dependencies/blocked_by`]);
    let currentBlockers = [];
    if (currentRes.status === 0 && currentRes.stdout.trim()) {
      try {
        currentBlockers = JSON.parse(currentRes.stdout);
      } catch (e) {
        currentBlockers = [];
      }
    }

    const desiredNumbers = new Set(item.blockerIssueNumbers);
    const currentNumbers = new Set(currentBlockers.map(b => b.number));

    // Xóa các cạnh cũ bị thừa / sai
    for (const cur of currentBlockers) {
      if (!desiredNumbers.has(cur.number)) {
        console.log(`  - Gỡ cạnh chặn sai trên #${item.number}: bỏ blocker #${cur.number}`);
        gh(['api', '--method', 'DELETE', `repos/${repo}/issues/${item.number}/dependencies/blocked_by/${cur.id}`]);
      }
    }

    // Thêm các cạnh chặn còn thiếu
    for (const wantedNum of desiredNumbers) {
      if (!currentNumbers.has(wantedNum)) {
        let blockerDbId = issueDbIdByNumber.get(wantedNum);
        if (!blockerDbId) {
          const bRes = gh(['api', `repos/${repo}/issues/${wantedNum}`, '--jq', '.id']);
          if (bRes.status === 0 && bRes.stdout.trim()) {
            blockerDbId = Number(bRes.stdout.trim());
            issueDbIdByNumber.set(wantedNum, blockerDbId);
          }
        }
        if (blockerDbId) {
          console.log(`  + Thêm cạnh chặn chuẩn trên #${item.number}: blocked by #${wantedNum}`);
          gh(['api', '--method', 'POST', `repos/${repo}/issues/${item.number}/dependencies/blocked_by`, '-F', `issue_id=${blockerDbId}`]);
        }
      }
    }
  }

  // 6. ĐÓNG CÁC TASK CŨ ĐÃ TINH GIẢN (TASK-13 đến TASK-23) NẾU CÒN MỞ
  console.log('\n6. Kiểm tra các Issues TASK cũ ngoài phạm vi 12 task...');
  for (const issue of existingIssues) {
    const match = issue.title.match(/^\[(TASK-\d+)\]/);
    if (match && issue.state === 'OPEN') {
      const taskId = match[1];
      const isStillActive = tasks.some(t => t.id === taskId);
      if (!isStillActive) {
        console.log(`  x Đóng task cũ: #${issue.number} ${issue.title}`);
        gh(['issue', 'close', String(issue.number), '--comment', 'Đã tinh giản và gộp vào lộ trình 12 task chuẩn cho đồ án IT008.']);
      }
    }
  }

  console.log('\n=====================================================');
  console.log('HOÀN TẤT ĐỒNG BỘ MILESTONES, ISSUES & BLOCKING GRAPH!');
  console.log('=====================================================');
}

run();
