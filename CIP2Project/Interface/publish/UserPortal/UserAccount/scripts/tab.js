function GetObj(objName){
if(document.getElementById){
return eval('document.getElementById("' + objName + '")');
}else if(document.layers){
return eval("document.layers['" + objName +"']");
}else{
return eval('document.all.' + objName);
}
}
function tabconAlt(index,tabName,tabNum){
var con = "con"+tabName;
var tab = "tab"+tabName;
for(var i=0;i<tabNum;i++){
if(GetObj(con+i)&&GetObj(tab+i)){
GetObj(con+i).style.display = 'none';
GetObj(tab+i).className = "";
}
}
if(GetObj(con+index)&&GetObj(tab+index)){
GetObj(con+index).style.display = 'block';
GetObj(tab+index).className = "vazn";
}
}