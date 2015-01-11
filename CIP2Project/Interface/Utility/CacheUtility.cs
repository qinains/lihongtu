//==============================================================================================================
//
// Class Name: CacheUtility
// Description: Ã·π©Cache≤Ÿ◊˜∑Ω∑®
// Author: ‘∑∑Â
// Contact Email: yuanfeng@lianchuang.com
// Created Date: 2006-04-05
//
//==============================================================================================================
using System;
using System.Web;
using System.Web.Services;

namespace Linkage.BestTone.Interface.Utility
{
	/// <summary>
	/// Summary description for CacheUtility.
	/// </summary>
	public class CacheUtility
	{
		public CacheUtility()
		{
		}

		/// <summary>
		/// …Ë÷√ª∫¥Ê
		/// </summary>
		/// <param name="context"></param>
		/// <param name="data"></param>
		/// <param name="name"></param>
		/// <param name="expiretime"></param>
		public static void Set( HttpContext context , 
			object data , 
			string name , 
			DateTime expiretime )
		{
			context.Cache.Insert( name , data , null , expiretime , TimeSpan.Zero );
		}


		/// <summary>
		/// ∂¡»°ª∫¥Ê
		/// </summary>
		/// <param name="context"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static object Get( HttpContext context , string name )
		{
			object data = new object();

			try
			{
				data = context.Cache.Get( name );
			}
			catch
			{
				data = null;
			}

			return data;
		}


        /// <summary>
        /// …Ë÷√ª∫¥Ê
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name="expiretime"></param>
        public static void Set(WebService SpecificWebService,
            object data,
            string name,
            DateTime expiretime)
        {
            SpecificWebService.Context.Cache.Insert(name, data, null, expiretime, TimeSpan.Zero);
        }


        /// <summary>
        /// ∂¡»°ª∫¥Ê
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object Get(WebService SpecificWebService, string name)
        {
            object data = new object();

            try
            {
                data = SpecificWebService.Context.Cache.Get(name);
            }
            catch
            {
                data = null;
            }

            return data;
        }
	}// End Class
}
