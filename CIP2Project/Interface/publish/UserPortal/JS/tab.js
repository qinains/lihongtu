function GetObj(objName){
if(document.getElementById){
return eval('document.getElementById("' + objName + '")');
}else if(document.layers){
return eval("document.layers['" + objName +"']");
}else{
return eval('document.all.' + objName);
}
}
function tabconA(index,flag){
for(var i=0;i<4;i++){
if(GetObj("conA"+i)&&GetObj("tabA"+i)){
GetObj("conA"+i).style.display = 'none';
GetObj("tabA"+i).className = "";
}
}
if(GetObj("conA"+index)&&GetObj("tabA"+index)){
GetObj("conA"+index).style.display = 'block';
GetObj("tabA"+index).className = "vazn";
}
}
function tabconB(index,flag){
for(var i=0;i<4;i++){
if(GetObj("conB"+i)&&GetObj("tabB"+i)){
GetObj("conB"+i).style.display = 'none';
GetObj("tabB"+i).className = "";
}
}
if(GetObj("conB"+index)&&GetObj("tabB"+index)){
GetObj("conB"+index).style.display = 'block';
GetObj("tabB"+index).className = "vazn";
}
}
function tabconC(index,flag){
for(var i=0;i<10;i++){
if(GetObj("conC"+i)&&GetObj("tabC"+i)){
GetObj("conC"+i).style.display = 'none';
GetObj("tabC"+i).className = "";
}
}
if(GetObj("conC"+index)&&GetObj("tabC"+index)){
GetObj("conC"+index).style.display = 'block';
GetObj("tabC"+index).className = "vazn";
}
}
function tabconD(index,flag){
for(var i=0;i<5;i++){
if(GetObj("conD"+i)&&GetObj("tabD"+i)){
GetObj("conD"+i).style.display = 'none';
GetObj("tabD"+i).className = "";
}
}
if(GetObj("conD"+index)&&GetObj("tabD"+index)){
GetObj("conD"+index).style.display = 'block';
GetObj("tabD"+index).className = "vazn";
}
}
function tabconE(index,flag){
for(var i=0;i<15;i++){
if(GetObj("conE"+i)&&GetObj("tabE"+i)){
GetObj("conE"+i).style.display = 'none';
GetObj("tabE"+i).className = "";
}
}
if(GetObj("conE"+index)&&GetObj("tabE"+index)){
GetObj("conE"+index).style.display = 'block';
GetObj("tabE"+index).className = "vazn";
}
}