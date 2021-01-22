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
2)Navigate to folder, that contains downloaded file
</br>
3) Run:
<code>docker-compose up</code>
Note: If the PostgreSQL container have not been able to start up in time for WebAPI container, then WebAPI container would reload and try to connect again
