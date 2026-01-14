# QFramework Addressables 扩展

为 [QFramework](https://github.com/liangxiegame/QFramework) 提供 Unity Addressables 系统支持，让你可以在 QFramework 的 **ResKit**、**UIKit** 和 **AudioKit** 中无缝使用 Addressables 资源加载。

[toc]

## ✨ 特性

- 🔌 **完全兼容** - 与 QFramework 原有 API 保持一致
- 📦 **三大模块支持** - ResKit、UIKit、AudioKit 全覆盖
- 🎯 **灵活的资源查询** - 支持单资源、多资源并集/交集加载
- 🎬 **场景管理** - 支持 Addressables 场景的同步/异步加载

## 📋 环境要求

- Unity 2022.3 或更高版本（本项目使用 Unity 2022.3.62f3）
- [QFramework](https://github.com/liangxiegame/QFramework) v1.0+
- Unity Addressables 1.22.3+

## 📁 项目结构

```
Assets/
├── QFrameworkWithAddressables/              # 核心插件（UPM 包）
│   ├── package.json                         # UPM 包配置
│   ├── CHANGELOG.md                         # 更新日志
│   ├── QFrameworkWithAddressables.asmdef    # 程序集定义
│   ├── RunTime/                             # 运行时代码
│   │   ├── ResKit/                          # ResKit 扩展
│   │   │   ├── AddressablesResCreator.cs    # 资源创建器（自动注册）
│   │   │   ├── AddressablesSingleRes.cs     # 单资源加载类
│   │   │   ├── AddressablesMultipleRes.cs   # 多资源加载类
│   │   │   └── AddressablesResLoaderExtensions.cs  # 场景加载扩展方法
│   │   ├── UIKit/                           # UIKit 扩展
│   │   │   ├── AddressablesPanelLoaderPool.cs    # Panel 加载器
│   │   │   └── UIKitWithAddressablesInit.cs      # UIKit 自动初始化
│   │   └── AudioKit/                        # AudioKit 扩展
│   │       ├── AddressablesAudioLoaderPool.cs    # Audio 加载器
│   │       └── AudioKitWithAddressablesInit.cs   # AudioKit 自动初始化
│   └── Samples~/                            # UPM 示例（通过 Package Manager 导入）
└── Samples/                                 # 示例代码
    ├── ResKit/                              # ResKit 示例
    │   └── Addressables/                    # Addressables 资源示例
    ├── UIKit/                               # UIKit 示例
    │   └── Addressables/                    # UI Prefab 示例
    └── AudioKit/                            # AudioKit 示例
        └── Addressables/                    # 音频资源示例
```

## 🔧 安装

1. 确保项目中已安装 QFramework
2. 在 UMP（Unity Package Manager）中使用以下命令进行 git 安装：

   ```
   https://github.com/zheliku/Addressables-Support-for-QFramework.git?path=Assets/QFrameworkWithAddressables
   ```

   或者直接将 `Assets/QFramework Extension/AddressablesSupport` 文件夹复制到你的项目中

3. 注释冲突文件

   为了避免与本插件冲突，请在使用前注释掉以下两个文件的内容：

   - `Assets/QFramework/Toolkits/SupportOldQF/Scripts/AudioKitWithResKitInit.cs`
   - `Assets/QFramework/Toolkits/SupportOldQF/Scripts/UIKitWithResKitInit.cs`

   这两个文件是 QFramework 原有的初始化脚本，会与本插件的 Addressables 加载方式（AudioKit、UIKit）产生冲突。

   打开上述两个文件，将所有代码用 `//` 注释掉，或者直接删除文件内容。

   ```csharp
   // using System;
   // using UnityEngine;
   
   // namespace QFramework
   // {
   //     public class AudioKitWithResKitInit
   //     {
   //         // ... 注释掉全部内容
   //     }
   // }
   ```

   完成以上步骤后，即可正常使用本插件。

4. 对使用 InpustSystem 的项目，需要将 Assets/QFramework/Toolkits/UIKit/Scripts/Resources/UIRoot.prefab 子物体中的 EventSystem 上的 Standalone Input Module 改为 InputSystemOiinputModule 组件。其他示例场景同理。

   <img src="https://raw.githubusercontent.com/zheliku/TyporaImgBed/main/ImgBed202601142042292.png" alt="image-20260114204223153" style="zoom:50%;" />

## 📖 使用方法

### ResKit - 资源加载

#### 资源名前缀规则

| 前缀       | 说明                           | 示例                           |
| ---------- | ------------------------------ | ------------------------------ |
| `addr://`  | 加载单个资源或场景             | `addr://Prefabs/Player`        |
| `addru://` | 多个 key 的并集 (Union)        | `addru://Enemies; Bosses`      |
| `addri://` | 多个 key 的交集 (Intersection) | `addri://Characters; Unlocked` |

> 多个 key 使用分号 `;` 分隔

#### 单资源加载

```csharp
var loader = ResLoader.Allocate();

// 同步加载
var prefab = loader.LoadSync<GameObject>("addr://Prefabs/Player");
var texture = loader.LoadSync<Texture2D>("addr://Textures/Background");

// 异步加载
loader.Add2Load<GameObject>("addr://Prefabs/Enemy", (success, res) =>
{
    if (success)
    {
        var enemy = res.Asset as GameObject;
        Instantiate(enemy);
    }
});
loader.LoadAsync();

// 使用完毕后回收
loader.Recycle2Cache();
```

#### 多资源加载

```csharp
var loader = ResLoader.Allocate();

// 并集加载 - 加载匹配任意 key 的所有资源
loader.Add2Load("addru://Enemies; Bosses", (success, res) =>
{
    var allAssets = res.GetAllAssets(); // 使用扩展方法获取所有资源
    foreach (var asset in allAssets)
    {
        Debug.Log(asset.name);
    }
});

// 交集加载 - 加载同时匹配所有 key 的资源
loader.Add2Load("addri://Characters; Unlocked", (success, res) =>
{
    var multiRes = res as AddressablesMultipleRes;
    foreach (var asset in multiRes.AllAssets)
    {
        Debug.Log(asset.name);
    }
});

loader.LoadAsync();
loader.Recycle2Cache();
```

#### 场景加载

```csharp
var loader = ResLoader.Allocate();

// 同步加载场景
loader.LoadAddressableSceneSync("GameScene");
loader.LoadAddressableSceneSync("GameScene", LoadSceneMode.Additive);
loader.LoadAddressableSceneSync("GameScene", LoadSceneMode.Additive, LocalPhysicsMode.Physics2D);

// 异步加载场景
loader.LoadAddressableSceneAsync("GameScene");
loader.LoadAddressableSceneAsync("GameScene", LoadSceneMode.Additive);

// 异步加载并获取回调
loader.LoadAddressableSceneAsync("GameScene", sceneInstance =>
{
    Debug.Log("Scene loaded: " + sceneInstance.Scene.name);
}, LoadSceneMode.Additive);

// 使用 AsyncOperationHandle 回调
loader.LoadAddressableSceneAsync("GameScene", LoadSceneMode.Additive, LocalPhysicsMode.None, handle =>
{
    handle.Completed += h =>
    {
        Debug.Log("Scene loaded: " + h.Result.Scene.name);
    };
});

loader.Recycle2Cache();
```

### UIKit - UI 面板

UIKit 会自动使用 Addressables 加载面板 Prefab。只需确保你的 UI Prefab 已设置为 Addressable 资源，Key 为面板类型名称即可。

```csharp
// 面板定义
public class UIHomePanel : UIPanel
{
    // ...
}

// 使用方式（与原有 API 完全一致）
// 确保 Addressable Key 为 "UIHomePanel"
UIKit.OpenPanel<UIHomePanel>();

// 异步打开
UIKit.OpenPanelAsync<UIHomePanel>();

// 关闭面板
UIKit.ClosePanel<UIHomePanel>();
```

#### Addressable 设置

为 UI Prefab 设置 Addressable：

1. 选中你的 UI Prefab
2. 在 Inspector 中勾选 "Addressable"
3. 将 Address 设置为面板类名（如 `UIHomePanel`）

### AudioKit - 音频

AudioKit 会自动使用 Addressables 加载音频资源。只需确保你的 AudioClip 已设置为 Addressable 资源即可。

```csharp
// 确保 AudioClip 的 Addressable Key 为 "home_bg"
AudioKit.PlayMusic("home_bg");

// 播放音效
AudioKit.PlaySound("click_sound");

// 播放语音
AudioKit.PlayVoice("voice_01", onBeganCallback: () =>
{
    Debug.Log("Voice started");
});

// 控制音频
AudioKit.StopMusic();
AudioKit.PauseMusic();
AudioKit.ResumeMusic();
AudioKit.StopAllSound();
```

#### Addressable 设置

为 AudioClip 设置 Addressable：

1. 选中你的 AudioClip
2. 在 Inspector 中勾选 "Addressable"
3. 将 Address 设置为音频名称（如 `home_bg`）

## ⚙️ 自动初始化

本扩展使用 `RuntimeInitializeOnLoadMethod` 特性自动初始化，无需手动调用。

- **ResKit**: 在 `BeforeSceneLoad` 阶段自动注册 `AddressablesResCreator`
- **UIKit**: 在 `AfterSceneLoad` 阶段自动设置 `AddressablesPanelLoaderPool`
- **AudioKit**: 在 `AfterSceneLoad` 阶段自动设置 `AddressablesAudioLoaderPool`

如需手动初始化，可以这样做：

```csharp
// 手动注册 ResCreator
ResFactory.AddResCreator<AddressablesResCreator>();

// 手动设置 UIKit
UIKit.Config.PanelLoaderPool = new AddressablesPanelLoaderPool();

// 手动设置 AudioKit
AudioKit.Config.AudioLoaderPool = new AddressablesAudioLoaderPool();
```

## 📝 注意事项

1. **资源名大小写敏感** - Addressables Key 区分大小写，请确保资源名与 Addressable Key 完全匹配
2. **场景资源** - 场景资源会被自动识别，无需特殊处理
3. **资源释放** - 通过 `ResLoader.Recycle2Cache()` 回收时会自动释放 Addressables 句柄
4. **同步加载** - 同步加载使用 `WaitForCompletion()`，在某些平台上可能会有性能影响，建议优先使用异步加载

## 🤝 兼容性

本扩展与 QFramework 原有的资源加载方式完全兼容，你可以混合使用：

```csharp
var loader = ResLoader.Allocate();

// Addressables 资源
var addrPrefab = loader.LoadSync<GameObject>("addr://Prefabs/Player");

// Resources 资源（原有方式）
var resPrefab = loader.LoadSync<GameObject>("resources://Player");

// AssetBundle 资源（原有方式）
var abPrefab = loader.LoadSync<GameObject>("assetbundle_name", "Player");

loader.Recycle2Cache();
```

## 📜 License

MIT License

## 🔗 相关链接

- [QFramework](https://github.com/liangxiegame/QFramework)
- [Unity Addressables](https://docs.unity3d.com/Manual/com.unity.addressables.html)
