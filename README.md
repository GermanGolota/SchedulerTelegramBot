# SchedulerTelegramBot
Telegram bot for receiving scheduled messages
<h1>Content:</h1>
</br>
<a href="#First">1) Installation</a>
</br>
<a href="#Content"><h2 id="First">1 - Installation</h2></a>
This application can be installed via the docker container:
</br>
1) Download compose file from <a href="https://drive.google.com/file/d/1-RHHPDJnGxs3kB9v2kEI1YFamKTm-e-I/view?usp=sharing">here</a>
</br>
2) Navigate to folder, that contains downloaded file
</br>
3) Open docker-compose.yml file and setup following settings:
</br>
<code>Token: *token*</code>
</br>
Your telegram bot token(can be receiver from the Bot Father)
</br>
<code>DownloadFilesLocationBase: *location*</code> 
</br>
Location to the temporary directory for files downloads
</br>
<code id="longPulling">UseLongPulling: *"true" or "false"*</code> 
</br>
Indicates, whether long-pulling or webhooks should be used.
</br>
<strong>Note: If you use LongPulling, then you would not have to specify webhook url, otherwise:</strong>
</br>
<code>Webhook: *your address*/api/message/update</code> 
</br>
Your address is the address to which telegram api should send updates
</br>
<strong>Note: If this address is unavailable, then telegram would not be able to send updates. In that case, I recommend to use <span><a href="#longPulling">LongPulling</a></span></strong>
</br>
4) Run:
<code>docker-compose up</code>
Note: If the PostgreSQL container have not been able to start up in time for WebAPI container, then WebAPI container would reload and try to connect again
