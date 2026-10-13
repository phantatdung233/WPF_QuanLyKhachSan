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
    'Mốc 1: Module Độc Lập (Tuần 1-3)',
    'Mốc 2: Ghép Luồng E2E & Nâng Cao (Tuần 4-5)',
    'Mốc 3: Chốt Demo & Feature Freeze (Tuần 6)'
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

  // 3. PARSE TASKS.MD
  console.log('\n📖 3. Phân tích cú pháp TASKS.md...');
  const tasksPath = path.join(process.cwd(), 'TASKS.md');
  if (!fs.existsSync(tasksPath)) {
    console.error('Không tìm thấy file TASKS.md!');
    process.exit(1);
  }

  const content = fs.readFileSync(tasksPath, 'utf8');
  const lines = content.split(/\r?\n/);
  const tasks = [];
  let currentTask = null;
  let collectingDoD = false;

  for (let i = 0; i < lines.length; i++) {
    const line = lines[i];

    const headerMatch = line.match(/^###\s+\[(TASK-\d+)\]\s+(.+)$/);
    if (headerMatch) {
      if (currentTask) {
        tasks.push(currentTask);
      }
      currentTask = {
        id: headerMatch[1].trim(),
        title: `[${headerMatch[1].trim()}] ${headerMatch[2].trim()}`,
        assignee: '',
        priority: 'Medium',
        labels: [],
        branch: '',
        milestone: '',
        dod: []
      };
      collectingDoD = false;
      continue;
    }

    if (!currentTask) continue;

    const assigneeMatch = line.match(/^-\s+\*\*Assignee\*\*:\s*(.+)$/i);
    if (assigneeMatch) {
      currentTask.assignee = assigneeMatch[1].trim();
      continue;
    }

    const priorityMatch = line.match(/^-\s+\*\*Priority\*\*:\s*(.+)$/i);
    if (priorityMatch) {
      currentTask.priority = priorityMatch[1].trim();
      continue;
    }

    const labelsMatch = line.match(/^-\s+\*\*Labels\*\*:\s*(.+)$/i);
    if (labelsMatch) {
      currentTask.labels = labelsMatch[1]
        .split(',')
        .map(l => l.replace(/[`\s]/g, ''))
        .filter(Boolean);
      continue;
    }

    const branchMatch = line.match(/^-\s+\*\*Branch\*\*:\s*`?([^`\r\n]+)`?/i);
    if (branchMatch) {
      currentTask.branch = branchMatch[1].trim();
      continue;
    }

    const milestoneMatch = line.match(/^-\s+\*\*Milestone\*\*:\s*(.+)$/i);
    if (milestoneMatch) {
      currentTask.milestone = milestoneMatch[1].trim();
      continue;
    }

    if (line.match(/^-\s+\*\*Definition of Done\*\*:/i)) {
      collectingDoD = true;
      continue;
    }

    if (collectingDoD) {
      const trimmed = line.trim();
      if (trimmed.startsWith('- [ ]') || trimmed.startsWith('- [x]')) {
        currentTask.dod.push(trimmed);
      } else if (line.startsWith('---') || line.startsWith('###') || line.startsWith('## ')) {
        collectingDoD = false;
      }
    }
  }

  if (currentTask) {
    tasks.push(currentTask);
  }

  console.log(`  ✓ Đã trích xuất thành công ${tasks.length} tasks từ TASKS.md.`);

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

  for (const task of tasks) {
    // Xác định username assignee nếu được cung cấp
    let assigneeUsername = '';
    if (task.assignee.includes('Member A') && memberA) assigneeUsername = memberA;
    else if (task.assignee.includes('Member B') && memberB) assigneeUsername = memberB;
    else if (task.assignee.includes('Member C') && memberC) assigneeUsername = memberC;

    // Xây dựng nội dung Body chuẩn với dòng ngắt thực sự (\n) và markdown giữ nguyên
    let body = `### Phân Công Nhiệm Vụ\n`;
    body += `- **Mã Task**: \`${task.id}\`\n`;
    body += `- **Thành viên đảm nhiệm**: **${task.assignee}**${assigneeUsername ? ` (@${assigneeUsername})` : ''}\n`;
    body += `- **Mức độ ưu tiên**: \`${task.priority}\`\n`;
    body += `- **Nhánh làm việc gợi ý**: \`${task.branch}\`\n`;
    body += `- **Milestone**: ${task.milestone}\n\n`;
    body += `### Definition of Done (Checklist Hoàn Thành)\n`;
    if (task.dod.length > 0) {
      body += task.dod.join('\n') + '\n\n';
    } else {
      body += `- [ ] Hoàn thành toàn bộ chức năng theo kế hoạch đề ra.\n\n`;
    }
    body += `---\n*Tự động khởi tạo từ \`TASKS.md\` qua GitHub Actions.*`;

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

  console.log('\n=====================================================');
  console.log('✅ HOÀN TẤT ĐỒNG BỘ TOÀN BỘ BACKLOG LÊN GITHUB!');
  console.log('=====================================================');
}

run();
