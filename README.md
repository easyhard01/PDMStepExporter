# PDM 物料检索 · STEP 批量导出工具（清华紫）

基于 **C# / WinForms** 的桌面工具：在 **SOLIDWORKS Enterprise PDM（EPDM）** 中按「物料编码 / 规格型号」**批量搜索**文件，命中的模型（`.sldprt` / `.sldasm`）自动在 **SolidWorks 2018** 中另存为 **STEP**（`.step`）文件。界面采用清华紫主题。

---

## 一、功能一览

| 模块 | 说明 |
|---|---|
| 自动登录 | 程序启动时自动用 Windows 身份 `LoginAuto` 登录 PDM（与 EdmBOM_CSharp 旧项目一致），失败可手动点「连接 PDM」 |
| 连接 PDM 库 | 自动列出本机已配置的 PDM 库视图；用户名留空 = 当前 Windows 身份登录（`LoginAuto`）；也可输入用户名/密码 |
| 批量输入 | 左侧文本框支持粘贴 Excel 多行 / 制表符分隔内容，每行一个编码或型号 |
| 搜索方式 | 单选「物料编码 / 规格型号 / 自动判断（先编码后型号，编码未命中再按型号）」 |
| 匹配方式 | 勾选「模糊匹配（包含）」用 `EdmVarOp_StringContains`，否则精确匹配 `EdmVarOp_StringEqualTo` |
| 结果表格 | 输入项 / 匹配字段 / 文件名 / 版本 / 本地路径 / 本地副本 / 可导出，按输入项去重 |
| 导出 STEP | 可「导出选中」或「导出全部」；需要 SolidWorks 运行；可选「用输入项命名 STEP」「导出前 Get 最新版本」「每个输入仅导出第一个匹配项」 |
| 复制源文件 | 可「复制选中」或「复制全部」；**不需要 SolidWorks**，直接把 .sldprt/.sldasm 复制到输出目录，适合 SolidWorks 不可用或只需取源文件的场景 |
| 日志 | 底部日志框彩色输出（成功绿色 / 错误红色），带进度条 |

**文件命名规则**：
- 勾选「用输入项命名 STEP」→ `物料编码（文件名）.step` 或 `规格型号（文件名）.step`（防止同一编码多个型号互相覆盖）
- 未勾选 → 直接 `文件名.step`

**默认输出目录**：`桌面\PDM_STEP导出`（可在 `App.config` 的 `OutputDir` 或界面修改）。

---

## 二、环境与运行

| 项 | 要求 |
|---|---|
| 开发环境 | Visual Studio 2015（工程格式 14.0） |
| 目标框架 | .NET Framework 4.8（若本机只有 4.5+，可在项目属性中改低，代码未用高版本特性） |
| SolidWorks | 2018（含 API 组件，默认安装即有） |
| EPDM | SOLIDWORKS PDM Professional 2018（EPDM 客户端） |

### 方式一：Visual Studio
1. 用 VS2015 打开 `PDMStepExporter.sln`。
2. 若引用三个互操作 DLL 报黄色感叹号，按下列路径重新添加引用（**嵌入互操作类型 = False**）：
   - `C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS PDM\EPDM.Interop.epdm.dll`
   - `C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist\SolidWorks.Interop.sldworks.dll`
   - `C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist\SolidWorks.Interop.swconst.dll`
3. 平台目标选择 **x64**（EPDM 与 SolidWorks 均为 64 位进程，工程已预设 Debug/Release x64）。
4. 直接 F5 运行。**请先启动 SolidWorks 并登录 PDM 库**（程序会优先附加到已运行的 SolidWorks 实例）。

### 方式二：命令行（无 VS 的机器）
在装有 SolidWorks + EPDM 的机器上，用 .NET 4.x 自带编译器（`C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe`）：

```bat
csc /nologo /target:winexe /platform:x64 /out:PDMStepExporter.exe ^
  /r:System.dll /r:System.Core.dll /r:System.Drawing.dll ^
  /r:System.Windows.Forms.dll /r:System.Configuration.dll ^
  /r:Microsoft.CSharp.dll /r:System.Xml.dll ^
  /r:"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS PDM\EPDM.Interop.epdm.dll" ^
  /r:"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist\SolidWorks.Interop.sldworks.dll" ^
  /r:"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist\SolidWorks.Interop.swconst.dll" ^
  PDMStepExporter\Program.cs PDMStepExporter\Form1.cs PDMStepExporter\Form1.Designer.cs ^
  PDMStepExporter\EdmClient.cs PDMStepExporter\EdmSearchItem.cs PDMStepExporter\SwExporter.cs ^
  PDMStepExporter\Controls\Theme.cs PDMStepExporter\Controls\GradientPanel.cs ^
  PDMStepExporter\Controls\FlatButton.cs PDMStepExporter\Controls\PurpleProgressBar.cs ^
  PDMStepExporter\Properties\AssemblyInfo.cs
```

> 编译产物 `PDMStepExporter.exe` 需与 `App.config`（生成时复制为 `PDMStepExporter.exe.config`）同目录。**注意：本工程设置了 `useLegacyV2RuntimeActivationPolicy="true"`，EPDM 互操作程序集是 .NET 2.0 目标，去掉该设置会加载失败。**

---

## 三、使用步骤

1. **自动登录**：程序启动后自动用 Windows 身份登录 PDM（`LoginAuto`），右上角状态变绿即成功。若自动登录失败，可手动选库后点「连接 PDM」。
2. **输入**：在批量输入框粘贴编码/型号（每行一个），选择搜索字段与匹配方式。
3. **搜索**：点「开始搜索」，右侧表格列出命中项（含本地路径与"是否已有本地副本"）。
4. **导出（二选一）**：
   - **导出 STEP**：需要 SolidWorks 运行。点「导出选中为 STEP」或「导出全部为 STEP」，程序自动 Get 最新版 → SolidWorks 打开 → SaveAs .STEP。
   - **复制源文件**：不需要 SolidWorks。点「复制选中源文件」或「复制全部源文件」，直接把 .sldprt/.sldasm 复制到输出目录，拿到文件后可自己在 SolidWorks 里另存 STEP。

---

## 四、App.config 配置说明

```xml
<add key="VaultName"      value="" />     <!-- 上次使用的库名（留空不预选） -->
<add key="VariableCode"   value="物料编码" />  <!-- ★ 与 PDM 数据卡变量名一致 -->
<add key="VariableSpec"   value="规格型号" />  <!-- ★ 与 PDM 数据卡变量名一致 -->
<add key="OutputDir"      value="" />     <!-- 输出目录（留空=桌面\PDM_STEP导出） -->
<add key="UseInputName"   value="True" /> <!-- 用输入项命名 STEP -->
<add key="FuzzyMatch"     value="False" /><!-- 模糊匹配（包含） -->
<add key="Recursive"      value="True" /> <!-- 递归搜索子文件夹 -->
<add key="GetLatest"      value="True" /> <!-- 导出前 Get 最新版本 -->
<add key="FirstOnly"      value="True" /> <!-- 每个输入仅导出第一个匹配项 -->
<add key="SearchMode"     value="2" />    <!-- 0=物料编码 1=规格型号 2=自动判断 -->
```

> ★ **最重要的修改点**：「物料编码」「规格型号」必须与你们 EPDM 库中**数据卡变量名完全一致**（大小写敏感）。若不一致，搜索会查不到任何结果。修改 `App.config` 后重启程序即可，界面默认值也会随之刷新。

---

## 五、技术要点（API 依据）

- **库列举**：`new EdmVault5()` → `(IEdmVault8)` → `GetVaultViews(out views, false)` → `mbsVaultName`（官方 Get File Information 示例）。
- **登录**：`LoginAuto(vaultName, hwnd)` 走 Windows 身份；`IEdmVault7.RootFolder.ID` 取根文件夹。
- **搜索**：搜索对象用 `dynamic`（兼容不同 EPDM 版本的接口差异）：
  ```csharp
  dynamic search = _vault.CreateSearch();
  search.SetToken(EdmSearchToken.Edmstok_FindFiles, true);
  search.SetToken(EdmSearchToken.Edmstok_FindFolders, false);
  search.SetToken(EdmSearchToken.Edmstok_Recursive, recursive);
  search.BeginAND();
  object varName = variableName; object varValue = term;
  search.AddVariable2(ref varName, ref varValue,
      fuzzy ? (int)EdmVarOp.EdmVarOp_StringContains    // 106 包含
            : (int)EdmVarOp.EdmVarOp_StringEqualTo);   // 100 精确
  search.EndAND();
  ```
  遍历 `GetFirstResult()` / `GetNextResult()`，结果强转 `IEdmFile5`。
- **本地路径**：先 `IEdmVault5.GetFolderFromID(parentFolderID).LocalPath` 取真实父目录（支持子文件夹），再加文件名；失败兜底 `IEdmFile5.GetLocalPath(rootFolderId)`。
- **Get 最新版本**：`IEdmFile5.GetFileCopy(0, true)`（0=最新，true=强制），代码用动态绑定调用以兼容版本差异。
- **SolidWorks 导出**：
  ```csharp
  ModelDocExtension.SaveAs(stepPath,
      (int)swSaveAsVersion_e.swSaveAsCurrentVersion,        // 0
      (int)swSaveAsOptions_e.swSaveAsOptions_Silent,        // 1
      null, ref errors, ref warnings);                      // .STEP 扩展名触发导出
  ```
  连接：优先 `Marshal.GetActiveObject("SldWorks.Application")` 附加已运行的 SolidWorks；否则 `new SldWorks()` 启动。打开模型用 `OpenDoc6(path, docType, swOpenDocOptions_Silent, ...)`，docType 为 `swDocPART` / `swDocASSEMBLY`。

---

## 六、已知限制与真机验证清单

本工程在**不具备 SolidWorks / EPDM 的开发机上**完成，已做两类验证，但**未在真机联调**，以下几点需要你在真机确认：

1. **变量名**：`App.config` 中「物料编码 / 规格型号」必须与你们库的数据卡变量名一致（最常见的不匹配原因）。
2. **搜索接口版本**：代码已对版本差异大的调用（`BeginAND`/`AddVariable2`/`EndAND`/`Logout`/`GetFolderFromID`）统一使用 **dynamic 动态绑定**，编译期不检查接口版本，运行时自动适配。若运行时仍报"找不到方法"，说明你的 EPDM 版本搜索条件 API 形态完全不同，可改用旧式 `AddCondition`：
   ```csharp
   // 备选方案（旧式 AddCondition，适用于更老的 EPDM 版本）
   search.AddCondition(variableName, (int)EdmSearchOperation.EdmSo_Equal, term);
   ```
   只需替换 `EdmClient.Search` 中 `BeginAND()..EndAND()` 之间的三行。
3. **`GetFolderFromID` / `folder.LocalPath`**：代码已做 try-catch 兜底（退回根文件夹拼接），若 2018 版个别成员不存在不会崩溃，仅子文件夹路径可能不准，此时看日志即可定位。
4. **登录方式**：若贵司库不允许 Windows 身份登录，请填用户名/密码（动态绑定 `Login`），仍失败请检查库“登录方式”配置。
5. **进程位数**：EPDM/SolidWorks 是 64 位进程，务必用 x64 编译并运行，不要在 x86 下运行。
6. **首次运行**：Windows 防火墙可能拦截 SolidWorks COM 调用，若导出无反应请检查 SolidWorks 是否已启动、EPDM 是否已登录。

## 七、目录结构

```
PDMStepExporter/
├── PDMStepExporter.sln
├── README.md
└── PDMStepExporter/
    ├── PDMStepExporter.csproj
    ├── App.config
    ├── Program.cs                  # 入口（STAThread）
    ├── Form1.cs                    # 主窗体逻辑（搜索/导出/日志/配置）
    ├── Form1.Designer.cs           # 界面布局（清华紫主题）
    ├── EdmClient.cs                # EPDM 封装：列库/登录/搜索/Get
    ├── EdmSearchItem.cs            # 搜索结果模型
    ├── SwExporter.cs               # SolidWorks 封装：连接/打开/SaveAs STEP
    ├── Controls/
    │   ├── Theme.cs                # 清华紫调色板 + 圆角 + 占位符
    │   ├── GradientPanel.cs        # 紫色渐变头部面板
    │   ├── FlatButton.cs           # 圆角自绘按钮
    │   └── PurpleProgressBar.cs    # 紫色渐变进度条
    ├── Properties/AssemblyInfo.cs
    └── docs/ui_preview.png         # 界面预览图（本机渲染验证）
```

---

*开发验证：本机已完成 ① csc.exe 全量源码编译（0 警告）② 窗体离屏渲染截图核对布局 ③ 关键 API 逐条对照官方文档。真机（含 SolidWorks 2018 + EPDM）联调待部署环境完成。*
