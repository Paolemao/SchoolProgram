using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{
    class MyList<T>
    {
        T[] testArray;
        int size;
        public MyList()
        {
            size = 0;
            testArray = new T[8];
        }
        public int Count
        {
            get { return size; }
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < size)
                {
                    return testArray[index];
                }
                return default(T);
            }
            set
            {
                if (index >=0 && index < size)
                {
                    testArray[index] = value;
                }
            }
        }

        public void Add(T testNum)
        {
            if (size==testArray.Length)
            {
                T[] temp = new T[testArray.Length * 2];
                testArray.CopyTo(temp,0);
                testArray = temp;
            }
            testArray[size] = testNum;
            size++;
        }
        public void Remove(T testNum)
        {
            int index = -1;
            for (int i = 0; i < size; i++)
            {
                if (testArray[i].Equals(testNum) )
                {
                    index = i;
                }
                RemoveAt(index);
            }

        }
        public void RemoveAt(int remove_index)
        {
            if (remove_index<0||remove_index>=size)
            {
                return;
            }
            int j = 0;
            T[] temp2 = new T[testArray.Length];
            testArray.CopyTo(temp2, 0);
            for (int i = 0; i < size; i++)
            {
                if (i == remove_index)
                {
                    j++;
                    testArray[i] = testArray[i + j];
                    size--;
                }
                testArray[i] = temp2[i + j];
            }
            ////方法2：Array.Copy方法
            //Array.Copy(testArray,remove_index+1,testArray,remove_index,size-remove_index-1);
            //size--;

        }
        public void Insert(int insert_index, T testNum)
        {
  
            int j = 0;
            T[] temp2 = new T[testArray.Length];
            testArray.CopyTo(temp2, 0);
            for (int i = 0; i < size; i++)
            {

                if (i == insert_index)
                {
                    testArray[i] = testNum;
                    j++;
                    size++;
                }
                else
                {
                    testArray[i] = temp2[i - j];
                }
            }
        }
        public T Get(int index)
        {
            return testArray[index];
        }
        public void Set(int index, T testNum)
        {
            testArray[index] = testNum;
        }

    }
}
