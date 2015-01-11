using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Utility
{
    public class PageSearch
    {
        private Int32 _currentIndex = 0;
        private Int32 _pageSize = 10;
        private Int32 _recordCount;
        private String _sortField;

        public PageSearch()
        {

        }

        public Int32 CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }
        public Int32 PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        public Int32 RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }
        public String SortFields
        {
            get { return _sortField; }
            set { _sortField = value; }
        }
    }
}
