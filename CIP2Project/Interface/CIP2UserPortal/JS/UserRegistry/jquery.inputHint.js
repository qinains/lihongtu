 ;(function($){
	$.fn.inputHint=function(options){
		var defaults={
			hint:"请输入内容"
		};
		if(options)
			$.extend(defaults, options)
		var $input=$(this);
		var html='<pre style="position:absolute;left:'+($input.position().left+5)+'px;top:'+($input.position().top+5)+'px;color:#999;font-size:14px">'+defaults.hint+'</pre>';
		$input
			.after(html)
			.focus(function(){	
				$input.siblings("pre").hide()
			})
			.blur(function(){
				if($input.val()=="")	
					$input.siblings("pre").show()
			})
	}
})(jQuery);