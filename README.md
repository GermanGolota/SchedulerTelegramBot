# SchedulerTelegramBot
Telegram bot for receiving scheduled messages
<h1 id="Content">Content:</h1>
</br>
<a href="#First">1) Installation</a>
<a href="#Second">2) Technology used</a>
<a href="#Third">3) Live version</a>
<a href="#Fourth">4) Preview</a>
</br>
<a href="#Content"><h2 id="First">1 - Installation</h2></a>
</br>
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
<a href="#Content"><h2 id="Second">2 - Technology used</h2></a>
<ul>
  <li>
    DB - PostgreSQL
  </li>
   <li>
    WebAPI - ASP.NET Core
  </li>
   <li>
    Deployment - Docker
  </li>
</ul>
</br>
<a href="#Content"><h2 id="Third">3 - Live version</h2></a>
Live version is hopefully available at @ScheduletTelegramBot, otherwise you can refer to the next section
<a href="#Content"><h2 id="Fourth">4 - Preview</h2></a>
<strong>Addition of schedule</strong>
<img src="https://user-images.githubusercontent.com/64675654/105529382-ff4adc00-5cee-11eb-877b-6d1d1d8ecb55.png"></img>
<strong>Operation may be scheduled on any time, that is supported by the cron value</strong>
<img src="https://user-images.githubusercontent.com/64675654/105529543-33260180-5cef-11eb-84b5-01259ecd6d22.png"></img>
<strong>To create your own schedule, you would need to create a json file in describe or use a built-in builder, that is hopefully coming soon</strong>
