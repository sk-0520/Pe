/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var loadPath = "Pe\\PeMain\\doc\\changelog.xml";
var savePath = "Update\\update.html";
var typeMap = {
	'features':  '機能',
	'fixes':     '修正',
	'developer': '開発',
	'note':      'メモ'
};

var xml = WScript.CreateObject('MSXML.DOMDocument');
xml.load(loadPath);

var stream = WScript.CreateObject('ADODB.Stream');
stream.Mode = 3;
stream.Type = 2;
stream.Charset = 'UTF-8';
stream.Open();

stream.WriteText('<html>');
stream.WriteText('<head>');
stream.WriteText('<title>Pe Update: Change log</title>');
stream.WriteText('<style>');
stream.WriteText('@import url("update.css")');
stream.WriteText('</style>');
stream.WriteText('</head>');
stream.WriteText('<body>');

var log = xml.getElementsByTagName('log')[0];
var version = log.getAttribute('version');
var date    = log.getAttribute('date');
stream.WriteText('<h1>Pe: ' + date + ', ' + version + '</h1>');

stream.WriteText('<dl>');
var list = log.getElementsByTagName('ul');
for(var i = 0; i < list.length; i++) {
	var type = list[i].getAttribute('type');

	stream.WriteText("<dt class='" + type + "'>" + typeMap[type] + '</dt>');
	stream.WriteText('<dd>');
	stream.WriteText('<ul>');
	var items = list[i].getElementsByTagName('li');
	for(var j = 0; j < items.length; j++) {
		stream.WriteText('<li>' + items[j].text + '</li>');
	}
	stream.WriteText('</ul>');
	stream.WriteText('</dd>');
}
stream.WriteText('</dl>');

stream.WriteText('</body>');
stream.WriteText('</html>');

stream.SaveToFile(savePath, adSaveCreateOverWrite);
stream.Close();

