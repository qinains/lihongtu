using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Utility
{
    public interface MAC
    {
        String getName();
        int getBlockSize();
        void init(byte[] key);
        void update(byte[] foo, int start, int len);
        void update(int foo);
        byte[] doFinal();
    }
}
