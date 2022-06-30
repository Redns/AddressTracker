# AddressTracker

![version: v1.0.0 (shields.io)](https://img.shields.io/badge/release-v1.0.0-green) ![version: v1.0.0 (shields.io)](https://img.shields.io/badge/.net-6.0-orange) ![version: v1.0.0 (shields.io)](https://img.shields.io/badge/License-MIT-blue)

### Background

通过 `ssh` 我们可以在没有任何外设的情况下连接 Linux 主机，但用户的个人主机很难取得固定的公网 IP，而且某些局域网（如校园网）也不允许我们固定自己的局域网 IP，每一次重启后主机都可能获取不同的 IP 地址。在这种情况下，通过 `ssh` 连接主机非常麻烦

1. 通过 ifconfig 等命令获取主机 IP（需要屏幕）
2. 使用手机（或电脑）搭建热点，之后查看连接设备的 IP（不需要屏幕）

若我们将域名解析到主机 IP，而主机定时刷新域名解析记录，这样我们仅通过域名就可以直接连接主机，而不需要手动查看主机地址

![image-20220630211543994](http://imagebed.krins.cloud/api/image/2D4B02FH.png)

> 注意：目前仅支持 DnsPod 平台的域名

### Install

1. 前往 [Release](https://github.com/Redns/AddressTracker/releases) 下载压缩包

   ![image-20220630213032470](http://imagebed.krins.cloud/api/image/F2428N06.png)

2. 前往 [DnsPod 控制台](https://console.dnspod.cn/account/token/apikey) 获取密钥

   ![image-20220630213654204](http://imagebed.krins.cloud/api/image/6HVHFL4Z.png)

3. 修改 appsettings.json 中的配置

   ![image-20220630213744601](http://imagebed.krins.cloud/api/image/84B4P2PT.png)

   - DnsPod
     - ID：DNSPod Token 应用 ID
     - Token：DNSPod Token 应用 Token
   - TecentCloud
     - APPID：腾讯云 API 密钥应用 ID
     - SecretId：腾讯云 API 密钥 SecretId
     - SecretKey：腾讯云 API 密钥 SecretKey
   - Domain
     - Root：待解析的根域名（二级域名）
     - SubDomain：待解析的子域名，最终的解析域名为 {SubDomain}.{Root}
   - General
     - RefreshInterval：域名更新时间间隔（单位：ms）

4. 运行程序

   ```bash
   nohup dotnet AddressTracker.dll &
   ```

​       日志默认路径 logs/AddressTracker.log

### Usage

#### 开机自启动

为了保证主机重启后依然能自动更新 IP，我们需要让主机重启后自动运行 AddressTracker。下面以 Ubuntu 20.04 为例，演示如何设置开机自启动命令

1. 创建系统服务

   ```bash
   sudo nano /lib/systemd/system/scratch.service
   ```

   内容为

   ```bash
   [Unit]
   Description=scratch service
   After=network.target network-online.target syslog.target
   Wants=network.target network-online.target
   
   [Service]
   Type=simple
   
   ExecStart=/usr/bin/nohup dotnet {AddressTracker.dll 路径} &
   
   [Install]
   WantedBy=multi-user.target
   ```

2. 允许服务开机自启动

   ```bash
   systemctl enable scratch.service
   ```

   其他命令

   ```bash
   启动 sudo systemctl start scratch
   重启 sudo systemctl restart scratch
   停止 sudo systemctl stop scratch
   日志 sudo systemctl status scratch
   ```

   