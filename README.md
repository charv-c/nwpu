# Unity 2D 大富翁游戏

这是一个使用Unity 2D开发的大富翁游戏，包含完整的游戏逻辑和用户界面。

## 功能特性

- 🎲 掷骰子系统（带动画效果）
- 🎯 玩家棋子自动移动（方形路线）
- 🎮 完整的UI界面（主菜单、游戏界面、设置界面）
- 🎨 自动生成游戏板
- 🔊 音效支持
- 📊 骰子结果显示
- 🔄 游戏重置功能

## 脚本说明

### 核心脚本

1. **GameManager.cs** - 游戏总管理器
   - 协调所有游戏系统
   - 管理游戏状态
   - 处理音效设置

2. **BoardManager.cs** - 游戏板管理器
   - 管理玩家移动逻辑
   - 处理骰子结果
   - 控制游戏流程

3. **Dice.cs** - 骰子系统
   - 掷骰动画
   - 随机数生成
   - 音效播放

4. **Chess.cs** - 玩家棋子
   - 棋子移动效果
   - 到达动画
   - 视觉效果

### UI脚本

5. **UIManager.cs** - UI管理器
   - 管理所有界面显示隐藏
   - 处理用户交互
   - 控制玩家棋子显示

6. **BoardGenerator.cs** - 游戏板生成器
   - 自动生成方形游戏板
   - 创建点位和连线
   - 自定义游戏板样式

## 安装和设置

### 1. 场景设置

1. 创建一个新的2D场景
2. 添加以下GameObject到场景中：

```
- GameManager (添加 GameManager 脚本)
- BoardGenerator (添加 BoardGenerator 脚本)
- UIManager (添加 UIManager 脚本)
- PlayerChess (添加 Chess 脚本)
- Dice (添加 Dice 脚本)
```

### 2. UI设置

创建以下UI面板：

#### 主菜单面板 (MainMenuPanel)
- 开始游戏按钮
- 设置按钮
- 退出按钮

#### 游戏面板 (GamePanel)
- 掷骰子按钮
- 返回主菜单按钮
- 骰子结果文本

#### 设置面板 (SettingsPanel)
- 音效开关
- 音乐音量滑块
- 关闭按钮

### 3. 组件配置

#### GameManager 配置
- 设置 `maxBoardPoints` (建议20)
- 将UI组件拖拽到对应字段
- 将游戏对象拖拽到对应字段

#### BoardGenerator 配置
- 设置 `boardSize` (建议20)
- 设置 `boardSizeX` (建议10)
- 设置 `boardSizeY` (建议10)
- 可选择添加点位和连线预制体

#### Dice 配置
- 添加6个骰子图片到 `diceSprites` 列表
- 设置 `rollDuration` (建议1.5秒)
- 添加音效文件（可选）
- **重要**: 确保Dice对象有Image组件

#### UIManager 配置
- 将所有UI面板和按钮拖拽到对应字段
- 将PlayerChess对象拖拽到playerChess字段

## 使用方法

### 基本游戏流程

1. **启动游戏**
   - 运行场景，显示主菜单
   - 点击"开始游戏"进入游戏界面

2. **游戏操作**
   - 点击"掷骰子"按钮开始掷骰
   - 观看骰子动画和玩家移动
   - 使用其他按钮控制游戏

3. **游戏控制**
   - 返回主菜单：点击返回按钮

### 界面显示逻辑

- **主菜单**: 显示主菜单面板，隐藏其他界面和玩家棋子
- **游戏界面**: 显示游戏面板，隐藏其他界面，显示玩家棋子
- **设置界面**: 覆盖显示设置面板，不影响其他界面状态

### 自定义设置

#### 修改游戏板
```csharp
// 在BoardGenerator中修改
boardSize = 30; // 改变游戏板大小
boardSizeX = 12f; // 改变游戏板X轴大小
boardSizeY = 8f; // 改变游戏板Y轴大小
boardColor = Color.blue; // 改变游戏板颜色
```

#### 修改骰子动画
```csharp
// 在Dice中修改
rollDuration = 2f; // 改变动画时长
minRollCount = 10; // 改变最小滚动次数
maxRollCount = 20; // 改变最大滚动次数
```

#### 修改玩家移动
```csharp
// 在BoardManager中修改
moveDuration = 0.5f; // 改变每步移动时间
```

## 故障排除

### 常见问题

1. **玩家不移动**
   - 检查BoardManager中的boardPoints是否正确设置
   - 确认BoardGenerator已正确生成游戏板
   - 验证Dice组件的事件回调是否正确设置
   - **重要**: 确保GameManager中的playerChess字段已正确设置

2. **棋子只在原地缩放不移动**
   - 检查BoardManager中的playerChess引用是否正确设置
   - 确认playerChess字段指向的是玩家棋子对象，而不是BoardManager本身
   - 验证GameManager的SetupBoardManager方法中是否正确设置了playerChess引用

3. **UI不显示**
   - 检查Canvas设置
   - 确认UI组件已正确拖拽到脚本字段
   - 检查UIManager中的界面显示逻辑

4. **游戏板不生成**
   - 检查BoardGenerator组件是否启用
   - 确认generateOnStart设置为true

5. **骰子不显示**
   - 确保Dice对象有Image组件
   - 检查diceSprites列表是否包含6张图片
   - 查看Console窗口的调试信息
   - 可以调用Dice.CreateDefaultDiceSprites()创建测试图片
   - **重要**: 确保Dice Image字段指向Canvas下的Image对象

6. **音效不播放**
   - 检查AudioSource组件
   - 确认音效文件已正确导入

## 技术细节

### 架构设计

- **单例模式**: GameManager和BoardManager使用单例模式
- **事件系统**: 使用C#事件进行组件间通信
- **协程**: 使用协程处理动画和移动
- **组件化**: 每个功能模块独立成组件

### 性能优化

- 使用对象池管理频繁创建的对象
- 优化UI更新频率
- 合理使用协程避免阻塞主线程

## 版本信息

- Unity版本: 2022.3 LTS或更高
- 渲染管线: Universal Render Pipeline (URP)
- 目标平台: Windows, macOS, Linux, Android, iOS

## 许可证

本项目采用MIT许可证，详见LICENSE文件。

## 贡献

欢迎提交Issue和Pull Request来改进这个项目！

## 联系方式

如有问题或建议，请通过以下方式联系：
- 邮箱: [your-email@example.com]
- GitHub: [your-github-username]
