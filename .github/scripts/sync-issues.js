const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

// Helper executing gh CLI safely without /bin/sh interpolation
function gh(args, options = {}) {
  const res = spawnSync('gh', args, {
    encoding: 'utf8',
    env: { ...process.env, ...options.env },
    stdio: options.stdio || ['pipe', 'pipe', 'pipe'],
  });
  return res;
}

function run() {
  console.log('=====================================================');
  console.log('🚀 IT008 - SYNC TASKS TO GITHUB MILESTONES & ISSUES');
  console.log('=====================================================');

  const repo = process.env.GITHUB_REPOSITORY;
  const memberA = (process.env.MEMBER_A_GITHUB || '').trim();
  const memberB = (process.env.MEMBER_B_GITHUB || '').trim();
  const memberC = (process.env.MEMBER_C_GITHUB || '').trim();

  // 1. TẠO HOẶC KIỂM TRA 3 MILESTONES
  console.log('\n📌 1. Kiểm tra & Khởi tạo Milestones...');
  const milestones = [
    'Mốc 1: Khung Ứng Dụng, CSDL & Danh Mục Cơ Bản (Tuần 1-3)',
    'Mốc 2: Nghiệp Vụ Cốt Lõi, Sơ Đồ Phòng & Hóa Đơn (Tuần 3-5)',
    'Mốc 3: Báo Cáo Thống Kê & Chốt Demo (Tuần 5-6)'
  ];

  let existingMilestones = [];
  try {
    const msRes = gh(['api', `repos/${repo}/milestones`, '--jq', '.[].title']);
    if (msRes.status === 0 && msRes.stdout) {
      existingMilestones = msRes.stdout.split(/\r?\n/).filter(Boolean);
    }
  } catch (e) {
    console.warn('Không thể lấy danh sách milestones:', e.message);
  }

  for (const m of milestones) {
    if (existingMilestones.includes(m)) {
      console.log(`  ✓ Milestone đã tồn tại: "${m}"`);
    } else {
      console.log(`  + Đang tạo Milestone mới: "${m}"...`);
      gh(['api', `repos/${repo}/milestones`, '-f', `title=${m}`]);
    }
  }

  // 2. KHỞI TẠO BỘ LABELS MÀU CHUẨN
  console.log('\n🎨 2. Kiểm tra & Khởi tạo Labels màu...');
  const labelColorMap = {
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
    'performance': 'd4c5f9',
    'async': '006b75',
    'integration': '1d76db',
    'e2e-testing': '0366d6',
    'refactor': 'fbca04',
    'clean-code': '2cbe4e',
    'qa': 'd93f0b',
    'bugfix': 'd73a4a',
    'hardening': 'b60205',
    'docs': '0075ca',
    'presentation': 'a2eeef',
    'demo': '7057ff',
    'devops': '000000',
    'git': 'f34f29',
    'seed-data': 'bfdadc',
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
  console.log('  ✓ Đã cập nhật xong bộ nhãn màu chuẩn.');

  // 3. PARSE docs/tasks/*.md
  console.log('\n📖 3. Phân tích cú pháp docs/tasks/*.md...');
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

    // Tiêu đề task: "# [TASK-01] Khởi tạo Solution..."
    const headerLine = lines.find(l => /^#\s+\[TASK-\d+\]/.test(l));
    if (!headerLine) {
      console.warn(`     [WARN] Bỏ qua ${fileName}: không có heading "# [TASK-xx]".`);
      continue;
    }
    const headerMatch = headerLine.match(/^#\s+\[(TASK-\d+)\]\s+(.+)$/);

    // Metadata dạng "**Key**: value" ở phần đầu file.
    const getMeta = (key) => {
      const ln = lines.find(l => l.startsWith(`**${key}**`));
      return ln ? ln.split(':').slice(1).join(':').trim() : '';
    };

    // Bỏ markdown (backtick, **, ngoặc kép) để lấy giá trị thuần.
    const sanitize = (v) => (v || '').replace(/[`*"']/g, '').trim();

    // Định dạng metadata: "**Labels**: `a`, `b`"
    const labels = (getMeta('Labels') || '')
      .split(',')
      .map(l => l.replace(/[`*\s]/g, ''))
      .filter(Boolean);

    tasks.push({
      id: headerMatch[1].trim(),
      title: `[${headerMatch[1].trim()}] ${headerMatch[2].trim()}`,
      assignee: getMeta('Thành viên đảm nhiệm').replace(/\*\*/g, '').trim(),
      priority: sanitize(getMeta('Mức độ ưu tiên')) || 'Medium',
      labels,
      branch: sanitize(getMeta('Nhánh làm việc')),
      milestone: getMeta('Milestone'),
      dod: []
    });
  }

  console.log(`  ✓ Đã trích xuất thành công ${tasks.length} tasks từ docs/tasks/.`);

  // 4. LẤY DANH SÁCH ISSUES HIỆN CÓ ĐỂ TẠO MỚI HOẶC SỬA LỖI (REPAIR/UPDATE)
  console.log('\n🔄 4. Đồng bộ GitHub Issues (Tạo mới & Sửa lỗi hiển thị)...');
  let existingIssues = [];
  try {
    const listRes = gh(['issue', 'list', '--state', 'all', '--limit', '200', '--json', 'number,title']);
    if (listRes.status === 0 && listRes.stdout) {
      existingIssues = JSON.parse(listRes.stdout);
    }
  } catch (e) {
    console.warn('Không thể lấy danh sách issues hiện tại:', e.message);
  }

  // Helper: build detailed issue body from docs/tasks/<TASK-ID>.md (source of truth),
  // decorated with info table. Falls back to a short body if the task file is unreadable.
  function buildBody(task) {
    const detailPath = path.join(process.cwd(), 'docs', 'tasks', `${task.id}.md`);
    if (fs.existsSync(detailPath)) {
      try {
        const raw = fs.readFileSync(detailPath, 'utf8');
        const rawLines = raw.split(/\r?\n/);
        const getMeta = (key) => {
          const ln = rawLines.find(l => l.startsWith(`**${key}**`));
          return ln ? ln.split(':').slice(1).join(':').trim() : '';
        };
        const member = getMeta('Thành viên đảm nhiệm');
        const branch = getMeta('Nhánh làm việc');
        const blocked = getMeta('Blocked by');
        const startIdx = rawLines.findIndex(l => l.startsWith('## 1.'));
        const endIdx = rawLines.findIndex(l => l.startsWith('*Tài liệu tự động'));
        let sections = '';
        if (startIdx !== -1) {
          sections = rawLines.slice(startIdx, endIdx === -1 ? undefined : endIdx).join('\n').replace(/\n+$/, '');
        }
        const labelsStr = task.labels.map(l => `\`${l}\``).join(', ');
        const priority = task.priority;
        // Find canonical issue number from docs metadata "**GitHub Issue**: [#NN]"
        const issueLine = getMeta('GitHub Issue');
        const issueNumMatch = issueLine.match(/#(\d+)/);
        const issueNum = issueNumMatch ? issueNumMatch[1] : '';
        return [
          `> Part of SPEC #77 · Docs: \`docs/tasks/${task.id}.md\` · Spec: \`docs/specs/hotel-management-system.md\``,
          '',
          `# ${task.title}`,
          '',
          '## Thông tin Task',
          '',
          '| Trường | Giá trị |',
          '|---|---|',
          `| Mã Task | \`${task.id}\` |`,
          issueNum ? `| GitHub Issue | #${issueNum} |` : `| GitHub Issue | — |`,
          `| Thành viên | ${member} |`,
          `| Ưu tiên | ${priority} |`,
          `| Branch | ${branch} |`,
          `| Milestone | ${task.milestone} |`,
          `| Blocked by | ${blocked} |`,
          `| Labels | ${labelsStr} |`,
          '',
          '---',
          '',
          sections,
          '',
          '---',
          '',
          '## Tham khảo',
          '',
          '- SPEC tổng: #77 — Hotel Management System (WPF .NET 10 MVVM, SQL Server + ADO.NET)',
          '- Spec tổng: [`docs/specs/hotel-management-system.md`](https://github.com/phantatdung233/WPF_QuanLyKhachSan/blob/main/docs/specs/hotel-management-system.md)',
          `- Tài liệu gốc: [\`docs/tasks/${task.id}.md\`](https://github.com/phantatdung233/WPF_QuanLyKhachSan/blob/main/docs/tasks/${task.id}.md)`,
          '- Glossary thuật ngữ: [`GLOSSARY.md`](https://github.com/phantatdung233/WPF_QuanLyKhachSan/blob/main/GLOSSARY.md)',
          '',
          `*Đồng bộ từ \`${task.id}.md\` — mọi chỉnh sửa thực hiện trong \`docs/tasks/${task.id}.md\` rồi chạy workflow sync để cập nhật Issue${issueNum ? ` #${issueNum}` : ''}.*`,
          ''
        ].join('\n');
      } catch (e) {
        console.warn(`     [WARN] Không đọc được docs/tasks/${task.id}.md, dùng body rút gọn:`, e.message);
      }
    }
    // Fallback: body rút gọn dựng từ metadata của docs/tasks/<TASK-ID>.md
    let body = `### Phân Công Nhiệm Vụ\n`;
    body += `- **Mã Task**: \`${task.id}\`\n`;
    body += `- **Thành viên đảm nhiệm**: **${task.assignee}**\n`;
    body += `- **Mức độ ưu tiên**: \`${task.priority}\`\n`;
    body += `- **Nhánh làm việc gợi ý**: \`${task.branch}\`\n`;
    body += `- **Milestone**: ${task.milestone}\n\n`;
    body += `### Definition of Done (Checklist Hoàn Thành)\n`;
    if (task.dod.length > 0) {
      body += task.dod.join('\n') + '\n\n';
    } else {
      body += `- [ ] Hoàn thành toàn bộ chức năng theo kế hoạch đề ra.\n\n`;
    }
    body += `---\n*Tự động khởi tạo từ \`docs/tasks/${task.id}.md\` qua GitHub Actions.*`;
    return body;
  }

  for (const task of tasks) {
    // Xác định username assignee nếu được cung cấp
    let assigneeUsername = '';
    if (task.assignee.includes('Member A') && memberA) assigneeUsername = memberA;
    else if (task.assignee.includes('Member B') && memberB) assigneeUsername = memberB;
    else if (task.assignee.includes('Member C') && memberC) assigneeUsername = memberC;

    // Body chi tiết y chang docs/tasks/*.md (có trang trí bảng thông tin)
    const body = buildBody(task);

    // Tổng hợp labels
    const labels = [...task.labels];
    if (task.priority) {
      labels.push(`priority-${task.priority.toLowerCase()}`);
    }

    // Tìm xem issue này đã từng tồn tại chưa
    const existing = existingIssues.find(i => i.title.startsWith(`[${task.id}]`));

    if (existing) {
      // SỬA / CẬP NHẬT LẠI ISSUE ĐÃ CÓ (khắc phục hoàn toàn lỗi hiển thị cũ)
      console.log(`\n  ✏️ Cập nhật & sửa lỗi Issue #${existing.number}: ${task.title}`);
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
      if (res.status !== 0) {
        // Fallback nếu assignee không hợp lệ
        if (assigneeUsername && (res.stderr.includes('assignee') || res.stderr.includes('Could not resolve to a User'))) {
          console.warn(`     [WARN] Không thể gán @${assigneeUsername}, cập nhật không kèm assignee...`);
          const fallbackEditArgs = editArgs.filter((a, idx) => a !== '--add-assignee' && editArgs[idx - 1] !== '--add-assignee');
          res = gh(fallbackEditArgs);
        }
      }
      if (res.status === 0) {
        console.log(`     ✓ Đã sửa nội dung thành công cho #${existing.number}!`);
      } else {
        console.error(`     ❌ Lỗi cập nhật #${existing.number}:`, res.stderr);
      }
    } else {
      // TẠO MỚI ISSUE
      console.log(`\n  ➕ Tạo mới Issue: ${task.title}`);
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
      if (res.status !== 0) {
        // Fallback nếu assignee không hợp lệ
        if (assigneeUsername && (res.stderr.includes('assignee') || res.stderr.includes('Could not resolve to a User'))) {
          console.warn(`     [WARN] Không thể gán @${assigneeUsername}, tạo issue không kèm assignee...`);
          const fallbackCreateArgs = createArgs.filter((a, idx) => a !== '--assignee' && createArgs[idx - 1] !== '--assignee');
          res = gh(fallbackCreateArgs);
        }
      }
      if (res.status === 0) {
        console.log(`     ✓ Tạo thành công: ${res.stdout.trim()}`);
      } else {
        console.error(`     ❌ Lỗi tạo issue:`, res.stderr);
      }
    }
  }

  // 5. ĐÓNG CÁC TASK CŨ ĐÃ TINH GIẢN (TASK-13 đến TASK-23)
  console.log('\n🧹 5. Dọn dẹp & đóng các Issues cũ đã tinh giản...');
  for (const issue of existingIssues) {
    const match = issue.title.match(/^\[(TASK-\d+)\]/);
    if (match) {
      const taskId = match[1];
      const isStillActive = tasks.some(t => t.id === taskId);
      if (!isStillActive) {
        console.log(`  🗑️ Đóng task cũ không còn trong kế hoạch 12 task: #${issue.number} ${issue.title}`);
        gh(['issue', 'close', String(issue.number), '--comment', 'Đã tinh giản và gộp vào lộ trình 12 task mới cho đồ án IT008.']);
      }
    }
  }

  console.log('\n=====================================================');
  console.log('✅ HOÀN TẤT ĐỒNG BỘ TOÀN BỘ BACKLOG LÊN GITHUB!');
  console.log('=====================================================');
}

run();
