$(document).ready(function(){
								//获取页面导航值
								var p_text=$(".info_A").children("h4").children("strong").text();
								var d_text;
								//获取左侧dl列表数量值
								var dl_num=$(".boxA").children("dl").length;
								for(i=0;i<dl_num;i++)
								{
									//获取每个dl列表的dd数量值
									var dd_num=$(".boxA").children("dl").eq(i).children("dd").length;
									for(j=0;j<dd_num;j++)
										{	
											//获取每个dl列表的dd文本值
											d_text=$(".boxA").children("dl").eq(i).children("dd").eq(j).children().text();
											//如果页面导航值等于dd文本值
											if (d_text==p_text)
											{	
												$(".boxA").children("dl").eq(i).children("dd").eq(j).children().css({"color":"#cc0000","font-weight":"700"});
												$(".boxA").children("dl").eq(i).children("dd").show();
												return false;
											}
										}
									}
						   });

