# DcRat

**DcRat是我疫情那个学期的c#期末作业**  
[English](/README-EN.md)

## 介绍
##### 特点
- TCP 连接，连接稳定，使用证书校验，保证安全
- 可以通过链接动态获取服务器IP端口
- 多服务器，服务器多端口接收
- 通过.dll实现功能模块下发，可拓展性强
- 超小的客户端（大概40-50K）
- 采用msgpack传输数据（优于json等数据格式）
- 完善的日志记录

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
- 过启动
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
- 注册表编辑器（~~基本完成待整合~~，发现这个功能没有啥实际意义，懒得写了）
- 浏览器等密码提取，上网记录（暂时只支持chrome及edge，有需求的自己加路径）
- 重绘UI（贫穷，等有钱了找人设计）
- ~~hosts文件管理~~（没啥意义，太容易引起杀软提醒了）
- ……

## 编译
在Visual Studio 2019中打开解决方案即可编译。

## 下载
点击[此处](https://github.com/qwqdanchun/DcRat/releases/)下载最新版本

## 注意
我（簞純）对您由使用或传播等由此软件引起的任何行为和/或损害不承担任何责任。您对使用此软件的任何行为承担全部责任，并承认此软件仅用于教育和研究目的。下载本软件或软件的源代码，您自动同意上述内容。

## 感谢

* SiMay - [@koko](https://gitee.com/dWwwang/SiMayRemoteMonitorOS)
* Quasar - [@Quasar](https://github.com/quasar/Quasar)
* Lime-RAT - [@NYAN-x-CAT](https://github.com/NYAN-x-CAT/Lime-RAT)
* vanillarat - [@dannythesloth](https://dannythesloth.github.io/VanillaRAT/)
* StreamLibrary - [@Rut0](https://github.com/Rut0/StreamLibrary)
* SharpChromium- [@djhohnstein](https://github.com/djhohnstein/SharpChromium)
* AForge.NET - [@andrewkirillov](https://github.com/andrewkirillov/AForge.NET)
* AsyncRAT - [@NYAN-x-CAT](https://github.com/NYAN-x-CAT/AsyncRAT-C-Sharp)
* SimpleMsgPack.Net - [@ymofen](https://github.com/ymofen/SimpleMsgPack.Net/)
* SharpSploit - [@cobbr](https://github.com/cobbr/SharpSploit)
* 以及一些未记录的项目

## Donation

BTC: 17BuN4qd7tQ6CQqCUAhRkhgjVpy41WRkTc

## License
[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](/LICENSE)
This project is licensed under the MIT License - see the [LICENSE](/LICENSE) file for details
