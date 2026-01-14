使用前请按照如下方式配置：

## 1. 注释掉冲突文件

为了避免与本插件冲突，请在使用前注释掉以下两个文件的内容：

- `Assets/QFramework/Toolkits/SupportOldQF/Scripts/AudioKitWithResKitInit.cs`
- `Assets/QFramework/Toolkits/SupportOldQF/Scripts/UIKitWithResKitInit.cs`

这两个文件是 QFramework 原有的初始化脚本，会与本插件的 Addressables 加载方式（AudioKit、UIKit）产生冲突。

### 操作方法

打开上述两个文件，将所有代码用 `//` 注释掉，或者直接删除文件内容。

**示例：**

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
