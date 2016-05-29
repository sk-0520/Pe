/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var loadPath = "Pe\\PeMain\\doc\\model-changelogs.js";
var scriptPath = "Pe\\PeMain\\etc\\script\\changelog.js";
var styleCommonPath = "Pe\\PeMain\\etc\\style\\common.css";
var styleChangelogPath = "Pe\\PeMain\\etc\\style\\changelog.css";
var saveRcPath = "Changelog\\update-rc.html";
var saveReleasePath = "Changelog\\update-release.html";

eval(loadFile(loadPath));
eval(loadFile(scriptPath));

var updateVersions = [
	{
		isRc: true,
		path: saveRcPath
	},
	{
		isRc: false,
		path: saveReleasePath
	}
];

function isRc(update)
{
	return update.type == 'rc';
}

function isRcElement(node)
{
	return node.getAttribute('type') == 'rc';
}

function createStream()
{
	var stream = WScript.CreateObject('ADODB.Stream');
	stream.Mode = 3;
	stream.Type = 2;
	stream.Charset = 'UTF-8';
	stream.Open();

	return stream;
}

function loadFile(path)
{
	var stream = createStream();
	stream.LoadFromFile(path);
	return stream.ReadText;
}

function writeLine(stream, s)
{
	stream.WriteText(s);
	stream.WriteText("\r\n");
}

function writeHead(stream)
{
	var scriptStream = createStream();
	scriptStream.LoadFromFile(scriptPath);
	var scriptText = scriptStream.ReadText;
	
	var styleCommonStream = createStream();
	styleCommonStream.LoadFromFile(styleCommonPath);
	var styleCommonText = styleCommonStream.ReadText;
	
	var styleChangelogStream = createStream();
	styleChangelogStream.LoadFromFile(styleChangelogPath);
	var styleChangelogText = styleChangelogStream.ReadText;
	
	writeLine(stream, "<!DOCTYPE html>\r\n");
	writeLine(stream, '<html>');
	writeLine(stream, '<head>');
	writeLine(stream, '<meta charset="utf-8">');
	writeLine(stream, '<title>Pe Update: 最新バージョン更新情報</title>');
	writeLine(stream, '<script>');
	writeLine(stream, scriptText);
	writeLine(stream, 'window.onload = function() { makeIssueLink(); }');
	writeLine(stream, '</script>');
	writeLine(stream, '<style>');
	writeLine(stream, styleCommonText);
	writeLine(stream, '</style>');
	writeLine(stream, '<style>');
	writeLine(stream, styleChangelogText);
	writeLine(stream, '</style>');
	writeLine(stream, '</head>');
	writeLine(stream, '<body>');
}

function writeFoot(stream)
{
	writeLine(stream, '</body>');
	writeLine(stream, '</html>');
}

//個別履歴出力
for(var i = 0; i < updateVersions.length; i++) {
	var update = updateVersions[i];

	var stream = createStream();
	writeHead(stream);

	var targetLog = null;
	for(var j = 0; j < changelogs.length; j++) {
		if(changelogs[j].isRc) {
			if(update.isRc) {
				targetLog = changelogs[j];
				break;
			}
		} else if(!update.isRc) {
			targetLog = changelogs[j];
			break;
		}
	}
	if(!targetLog) {
		continue;
	}

	var log = makeChangeLogBlock(targetLog)
	writeLine(stream, log.title.toString())
	writeLine(stream, log.contents.toString())
	
	writeFoot(stream);

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}



