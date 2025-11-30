# 项目维护指南 | Project Maintenance Guide

本文档说明如何长期维护项目的文档和脚本。

---

## 📋 文档管理原则

### 1. 单一职责原则
每个文档应该只专注于一个主题:
- ✅ **好**: `quick-start.md` 专注于快速入门
- ❌ **差**: 一个文档包含入门、配置、测试、部署

### 2. 避免重复
如果多个地方需要相同信息,使用链接引用:
```markdown
详见 [开发工作流](development/workflow.md)
```
而不是复制粘贴内容。

### 3. 保持更新
- 功能变更时更新相关文档
- 废弃功能时移除或标记
- 定期审查文档准确性

### 4. 清晰的导航
- 每个目录都有 `README.md` 作为索引
- 主 `README.md` 提供项目总览
- 使用相对链接连接文档

---

## 📂 文档目录结构规范

```
docs/
├── README.md                # 文档中心索引
├── MAINTENANCE.md           # 本文档
├── development/            # 开发相关文档
│   ├── quick-start.md      # 快速入门
│   ├── workflow.md         # 工作流程
│   └── [其他开发文档]
├── testing/                # 测试相关文档
│   ├── overview.md         # 测试总览
│   └── [其他测试文档]
└── game-design/            # 游戏设计文档
    └── [设计文档]
```

### 添加新文档
1. **确定文档类型** - 开发/测试/设计?
2. **放入对应目录** - `docs/development/` 或 `docs/testing/` 等
3. **更新索引** - 在相应的 `README.md` 中添加链接
4. **交叉引用** - 在相关文档中添加链接

### 废弃文档
1. **不要直接删除** - 可能有外部链接引用
2. **添加废弃标记**:
   ```markdown
   > ⚠️ **已废弃**: 本文档已被 [新文档](path/to/new.md) 替代
   ```
3. **几个月后再删除** - 给使用者时间适应

---

## 🔧 脚本管理原则

### 1. 按功能分类
```
scripts/
├── test/       # 测试脚本 - 快速启动游戏测试
├── dev/        # 开发工具 - 自动化、分析等
└── archived/   # 废弃脚本 - 不再使用但保留
```

### 2. 命名规范
- 使用描述性名称: `autotest.sh` 而非 `at.sh`
- 测试脚本前缀: `test-*.{sh,cmd,ps1}`
- 工具脚本: 动词开头 `watch-and-test.sh`, `analyze-test.sh`

### 3. 脚本头部注释
每个脚本应包含:
```bash
#!/bin/bash
# autotest.sh - Automated testing script for OpenRA RA2 mod
#
# Usage: ./autotest.sh [mode] [automation]
#   mode: fast|normal (default: fast)
#   automation: auto|manual (default: manual)
#
# Examples:
#   ./autotest.sh fast auto
#   ./autotest.sh normal manual
```

### 4. 更新脚本文档
修改脚本时,同步更新:
- `scripts/README.md` - 脚本列表和用法
- 相关的开发文档

---

## 🗑️ 临时文件处理

### 哪些文件应该被忽略?

已在 `.gitignore` 中配置:
```gitignore
# AI工具临时文件
.serena/
.ra2_aliases

# 测试日志
test-logs/

# 临时文档
*_TEMP.md
*_OLD.md

# 编辑器临时文件
*.swp
*.swo
```

### 清理临时文件
```bash
# 定期清理测试日志(保留最近7天)
find test-logs/ -name "*.log" -mtime +7 -delete

# 清理编辑器临时文件
find . -name "*.swp" -o -name "*.swo" | xargs rm -f
```

---

## 📅 定期维护检查清单

### 每月检查
- [ ] 审查根目录,确保无新增临时文件
- [ ] 检查文档链接是否有效
- [ ] 更新过时的示例代码
- [ ] 清理旧的测试日志

### 每季度检查
- [ ] 审查所有文档,更新过时信息
- [ ] 检查脚本是否仍然工作
- [ ] 整合重复的文档内容
- [ ] 更新依赖和工具版本说明

### 重大更新时
- [ ] 更新 `README.md`
- [ ] 更新相关文档
- [ ] 测试所有脚本
- [ ] 更新 `CHANGELOG.md` (如果有)

---

## 🚫 禁止的做法

### 不要在根目录创建临时文件
```bash
# ❌ 错误
touch QUICK_FIX.txt
touch NEW_FEATURE_NOTES.md

# ✅ 正确
touch docs/development/new-feature.md
# 或者使用临时后缀
touch notes_TEMP.md  # .gitignore 会忽略
```

### 不要创建重复的文档
```bash
# ❌ 错误 - 创建多个相似文档
docs/quick-start.md
docs/getting-started.md
docs/quickstart.md

# ✅ 正确 - 只保留一个,其他的链接到它
```

### 不要留下未使用的脚本
```bash
# ❌ 错误 - 在根目录留下旧脚本
old-test.sh
backup-autotest.sh

# ✅ 正确 - 移动到归档或删除
mv old-test.sh scripts/archived/
rm backup-autotest.sh
```

---

## 🔄 重构文档/脚本流程

### 当需要重组时

1. **评估现状**
   ```bash
   # 列出所有文档
   find docs/ -name "*.md" | sort

   # 列出所有脚本
   find scripts/ -type f | sort
   ```

2. **规划新结构**
   - 画出目录树
   - 列出移动/合并/删除计划
   - 确定需要更新的链接

3. **执行重组**
   ```bash
   # 移动文件
   mv old-location new-location

   # 更新引用
   grep -r "old-location" docs/ scripts/
   ```

4. **验证**
   - 检查所有链接
   - 测试所有脚本
   - 确认 Git 状态正确

5. **提交**
   ```bash
   git add .
   git commit -m "Reorganize documentation and scripts

   - Move docs to docs/ directory
   - Move scripts to scripts/ directory
   - Update all references
   - Add maintenance guide"
   ```

---

## 💡 最佳实践建议

### 文档编写
1. **使用Markdown标准语法**
2. **提供代码示例**
3. **包含故障排除部分**
4. **保持简洁明了**

### 脚本开发
1. **添加错误处理**
2. **提供帮助信息** (`--help`)
3. **验证依赖** (检查必需的工具)
4. **输出清晰的消息**

### 版本控制
1. **有意义的提交消息**
2. **逻辑上的提交单元** (不要混合无关改动)
3. **定期推送**

---

## 📖 参考资源

### 文档编写规范
- [Markdown Guide](https://www.markdownguide.org/)
- [Google Developer Documentation Style Guide](https://developers.google.com/style)

### Shell脚本规范
- [Google Shell Style Guide](https://google.github.io/styleguide/shellguide.html)
- [ShellCheck](https://www.shellcheck.net/) - Shell脚本检查工具

---

## ❓ 常见问题

### Q: 我应该在哪里记录新功能的文档?
**A:** 根据功能类型:
- 开发相关 → `docs/development/`
- 测试相关 → `docs/testing/`
- 游戏设计 → `docs/game-design/`

并在相应的 `README.md` 中添加索引。

### Q: 旧脚本还能用,要删除吗?
**A:**
- 如果有更好的替代品,移到 `scripts/archived/`
- 标记为已废弃,几个月后删除
- 在 `scripts/README.md` 中说明替代方案

### Q: 文档太多了,怎么整理?
**A:**
1. 识别重复内容并合并
2. 过时内容标记废弃
3. 按主题重新组织
4. 更新索引和链接

### Q: 多个文档需要相同的信息怎么办?
**A:**
- 在一个地方详细说明
- 其他地方简要说明 + 链接
- 示例: "详见 [工作流文档](development/workflow.md#section)"

---

## 🎯 总结

**好的维护实践:**
- 📁 清晰的目录结构
- 📝 准确的文档
- 🔧 有用的脚本
- 🗑️ 及时清理临时文件
- 🔄 定期审查和更新

**保持项目整洁需要:**
- 遵循规范
- 定期维护
- 及时清理
- 持续改进

---

**维护良好的项目,开发更高效!** 🚀
