/// <summary>
/// 事件ID管理类，用于统一管理所有事件ID
/// 
/// 设计思路：
/// 1. 使用常量字符串，避免字符串硬编码
/// 2. 按模块分类管理，便于维护
/// 3. 统一命名规范，提高可读性
/// 
/// 命名规范：
/// 1. 游戏事件：Game + 动作（如GameStart）
/// 2. UI事件：UI + 动作（如UIOpen）
/// 3. 系统事件：System + 动作
/// 4. 业务事件：模块名 + 动作
/// 
/// 使用建议：
/// 1. 新增事件ID时，先在此类中定义常量
/// 2. 按模块分类添加，并添加注释说明用途
/// 3. 避免使用魔法字符串
/// </summary>
public static class EventIds
{
    // 游戏相关事件
    public const string GameStart = "GameStart";    // 游戏开始
    public const string GamePause = "GamePause";    // 游戏暂停
    public const string GameResume = "GameResume";  // 游戏恢复
    public const string GameOver = "GameOver";      // 游戏结束

    // UI相关事件
    public const string UIOpen = "UIOpen";          // UI打开
    public const string UIClose = "UIClose";        // UI关闭
    public const string UIUpdate = "UIUpdate";      // UI更新

    // 测试事件
    public const string TestEvent = "TestEvent";    // 测试事件

    // 可以按模块继续添加更多事件ID...
}
