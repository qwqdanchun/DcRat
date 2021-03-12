# DcRat-beta

**DcRat是我本学期的c#期末作业**

严格来说，这个项目从很久之前就开始准备了，很多小功能的代码早就写好的。这次正好趁着机会整合到一起，做一个小工具

**配套项目（用于免杀）：https://github.com/qwqdanchun/Crypter**

## 介绍
##### 特点
- TCP 连接（使用自签的ssl证书，保证安全），连接稳定
- 动态DNS（通过链接获取服务器IP端口）多服务器支持
- 多服务器，服务器多端口接收
- 通过.dll实现功能模块下发，可拓展性强
- 超小的客户端（目前在40多k，开发过程中不断变化）
- 采用msgpack传输数据（优于json等数据格式）
- 完善的日志记录（log及error以及各种反馈信息）

##### 功能
- 远程shell
- 远程桌面
- 远程摄像头
- 文件管理
- 进程管理
- 远程录音
- 窗口通知
- 发送文件
- 注入文件
- 发送通知
- 远程聊天
- 打开网站
- 修改壁纸
- 键盘记录
- 文件查找
- DDOS
- 加密勒索
- 关闭 Windows defender
- 锁屏
- 客户端关闭重启升级卸载
- 系统关机重启注销
- UAC提权
- 获取电脑详细信息
- 轮播图
- 自动任务
- 互斥锁
- 进程保护
- 屏蔽客户端
- 等等……

> 功能展示：https://www.bilibili.com/video/BV1hT4y1E7xT

##### 依赖

- 编译：vs2019
- 运行：
    - Server    .NET Framework 4.5
    - Client and others    .NET Framework 4.0

##### 支持
* 支持以下系统(32和64位)【要求带有.NET Framework 4.0 或更高版本 ([下载](https://www.microsoft.com/en-us/download/details.aspx?id=24872))】
  * Windows XP SP3
  * Windows Server 2003
  * Windows Vista
  * Windows Server 2008
  * Windows 7
  * Windows Server 2012
  * Windows 8/8.1
  * Windows 10

##### TODO
- 注册表编辑器（~~基本完成待整合~~，发现这个功能没有啥实际意义，如果想查看注册表，可以手动执行shell："reg export HKLM AppBkUp.reg"导出所有注册表，或者类似的操作来应付极少数需要处理注册表的情况）
- 聊天记录提取（微信基本完成，qq还没有思路）
- 浏览器等密码提取，上网记录（chrome更新太快了，跟不上）
- 重绘UI（贫穷，等有钱了找人设计）
- ~~hosts文件管理~~（没啥意义，太容易引起杀软提醒了，等找到无提示的方法再加）
- 语音视频的流传输
- 中继服务器
- ……

## 编译
在Visual Studio 2019中打开解决方案即可编译，如果出现错误请自行查看Nuget包是否有误。

## 注意
我（簞純）对您使用此软件可能执行的任何操作概不负责。您对使用此软件采取的任何措施承担全部责任。请注意，此应用程序仅用于教育目的，切勿被恶意使用。通过下载软件或软件的源代码，您自动接受此协议。

## 感谢

* SiMay - [@koko](https://gitee.com/dWwwang/SiMayRemoteMonitorOS)
* QuasarRAT - [@Quasar](https://github.com/quasar/QuasarRAT)
* Lime-RAT - [@NYAN-x-CAT](https://github.com/NYAN-x-CAT/Lime-RAT)
* vanillarat - [@dannythesloth](https://dannythesloth.github.io/VanillaRAT/)
* StreamLibrary - [@Rut0](https://github.com/Rut0/StreamLibrary)
* AForge.NET - [@andrewkirillov](https://github.com/andrewkirillov/AForge.NET)



## License
[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](/LICENSE)
This project is licensed under the MIT License - see the [LICENSE](/LICENSE) file for details
