# CoreBot
Bot Framework v4 core bot sample.

This bot has been created using [Bot Framework][1]

## Prerequisites
- [.NET Core SDK][4] version 2.1
	```bash
	# determine dotnet version
	dotnet --version
	```

# Deploy the bot to Azure
## Create Azure Web App Bot
- Go to [Azure portal][10]
- Create Azure's Web App Bot
- In section Channels add DirectLine
- Save DirectLine secret
## Publish from Visual Studio
- Download.PublishSettings file you find in the Azure's Web App pannel
- Copy the userPWD value
- Right click on the Project and click on "Publish..."
- Paste the password you just copied and publish

# Configuration of Bot
- Go to appsettings.json
- Change **MicrosoftAppId** and **MicrosoftAppPassword** using values from Azure settings
- Change **DirectLineSecret** with previously saved

# Conversation History 
- Go to **wwwroot**. Here you need:
  - default.html
  - js.js
## Initialization Without History
Request to a HomeController for token generation
```javascript
const res = await fetch('/api/token/generate', { method: 'POST' });
const { token, conversationId } = await res.json();
```
Initialize DirectLine object and save conversationId
```javascript
 dl = new DirectLine.DirectLine({ 
	token: token
});
createCookie("conversationId", conversationId, 365);
```
## Initialization With History
Request to a HomeController for token refreshing
```javascript
const response = await fetch('/api/token/refresh?conversationId=' + readCookie("conversationId"), { method: 'GET' });
const refreshData = await response.json();
```
Initialize chat with refreshing data
```javascript
dl = new DirectLine.DirectLine({
    token: refreshData.token,
    streamUrl: refreshData.streamUrl,
    webSocket: false,
    conversationId: readCookie("conversationId")
});
```
## Webchat Object
Then create WebChat object with static **userID**
```javascript
window.WebChat.renderWebChat({
	directLine: dl,
	userID: "default-user"
}, document.getElementById('webchat'));
```
[1]: https://dev.botframework.com
[4]: https://dotnet.microsoft.com/download
[10]: https://portal.azure.com
