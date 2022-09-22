using System;
using System.Collections.Generic;
using System.Linq;

namespace BytePackage
{
    public class BytePack
    {
        public const int _SI = 4;
        public const int _SC = 2;
        public const int _SF = 4;
        public const int _SD = 8;
        public const int _SL = 8; 

        private List<byte[]> _Bytes = new List<byte[]>();
        private int _ActiveIndex = 0;

        public BytePack() { }

        public static BytePack Read(byte[] allBytes)
        {
            return new BytePack(allBytes);
        }

        private BytePack(byte[] allBytes)
        {
            _Bytes = Unpackage(allBytes);
        }

        #region Packaging

        public static byte[] Package(params byte[][] byteArrays)
        {
            int si = _SI;
            int total = si * byteArrays.Length + byteArrays.Sum(t => t.Length);
            byte[] ret = new byte[total];
            int i = 0;

            for (int k = 0; k < byteArrays.Length; k++)
            {
                byte[] bytes = byteArrays[k];
                int len = bytes.Length;

                BitConverter.GetBytes(len).CopyTo(ret, i);
                i += si;

                bytes.CopyTo(ret, i);
                i += len;
            }

            return ret;
        }

        public static List<byte[]> Unpackage(byte[] bytes)
        {
            int si = _SI;

            List<byte[]> ret = new List<byte[]>();

            int i = 0;

            while (i < bytes.Length)
            {
                int len = BitConverter.ToInt32(bytes, i);
                i += si;

                byte[] bits = new byte[len];
                Array.Copy(bytes, i, bits, 0, len);
                ret.Add(bits);
                i += len;
            }

            return ret;
        }

        public int NumPackages { get { return _Bytes.Count; } }

        public byte[] Pack()
        {
            byte[] bytes = Package(_Bytes.ToArray());
            _Bytes.Clear();
            _Bytes.Add(bytes);

            return bytes;
        }

        public BytePack Unpack(int index = -1)
        {
            index = Advance(index);

            return new BytePack(_Bytes[index]);
        }

        public void Reset()
        {
            _ActiveIndex = 0;
        }


        #endregion

        // TODO: Make sure this is complete: bool, char, int, uint, short, ushort, long, double, float, DateTime, TimeSpan, Guid, string
        // TODO: Add methods for nullable objects.

        #region Add Item

        public void Add(byte[] values)
        {
            _Bytes.Add(values);
        }

        public void AddBool(bool value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddChar(char value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddShort(short value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddUShort(ushort value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddInt(int value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddUInt(uint value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddLong(long value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddULong(ulong value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddFloat(float value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddDouble(double value)
        {
            _Bytes.Add(BitConverter.GetBytes(value));
        }

        public void AddDateTime(DateTime value)
        {
            _Bytes.Add(DateTimeBytes(value));
        }

        public void AddTimeSpan(TimeSpan value)
        {
            _Bytes.Add(BitConverter.GetBytes(value.Ticks));
        }

        public void AddGuid(Guid value)
        {
            _Bytes.Add(value.ToByteArray());
        }

        public void AddString(string value)
        {
            byte[] bits = StringToBytes(value);
            _Bytes.Add(bits);
        }

        #endregion

        #region Add Items

        public void AddBools(bool[] values)
        {
            AddItems(1, BitConverter.GetBytes, values);
        }

        public void AddChars(char[] values)
        {
            AddItems(1, BitConverter.GetBytes, values);
        }

        public void AddShorts(short[] values)
        {
            AddItems(2, BitConverter.GetBytes, values);
        }

        public void AddUShorts(ushort[] values)
        {
            AddItems(2, BitConverter.GetBytes, values);
        }

        public void AddInts(int[] values)
        {
            AddItems(_SI, BitConverter.GetBytes, values);
        }

        public void AddUInts(uint[] values)
        {
            AddItems(_SI, BitConverter.GetBytes, values);
        }

        public void AddLongs(long[] values)
        {
            AddItems(sizeof(long), BitConverter.GetBytes, values);
        }

        public void AddULongs(ulong[] values)
        {
            AddItems(sizeof(ulong), BitConverter.GetBytes, values);
        }

        public void AddFloats(float[] values)
        {
            AddItems(_SF, BitConverter.GetBytes, values);
        }

        public void AddDoubles(double[] values)
        {
            AddItems(_SD, BitConverter.GetBytes, values);
        }

        public void AddDateTimes(DateTime[] values)
        {
            AddItems(_SL, DateTimeBytes, values);
        }

        public void AddTimeSpans(TimeSpan[] values)
        {
            AddItems(_SL, TimeSpanBytes, values);
        }

        public void AddGuids(Guid[] values)
        {
            AddItems(16, g => g.ToByteArray(), values);
        }

        public static byte[] TimeSpanBytes(TimeSpan t)
        {
            return BitConverter.GetBytes(t.Ticks);
        }
        public static TimeSpan FromBytesTimeSpan(byte[] b, int k)
        {
            return new TimeSpan(BitConverter.ToInt64(b, k));
        }

        public static byte[] DateTimeBytes(DateTime t)
        {
            return BitConverter.GetBytes(t.Ticks);
        }

        public static DateTime FromBytesDateTime(byte[] b, int k)
        {
            return new DateTime(BitConverter.ToInt64(b, k));
        }

        public void AddItems<T>(int stride, Func<T, byte[]> toBytes, T[] items)
        {
            int s = stride;
            int tot = items.Length * s;
            byte[] bits = new byte[tot];

            int i = 0;
            for (int k = 0; k < items.Length; k++)
            {
                Array.Copy(toBytes(items[k]), 0, bits, i, s);
                i += s;
            }

            _Bytes.Add(bits);
        }

        #endregion

        #region Read Item

        public bool ReadBool(int index = -1)
        {
            index = Advance(index);

            return BitConverter.ToBoolean(_Bytes[index], 0);
        }

        public char ReadChar(int index = -1)
        {
            index = Advance(index);

            return BitConverter.ToChar(_Bytes[index], 0);
        }

        public short ReadShort(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToInt16(_Bytes[index], 0);
        }

        public ushort ReadUShort(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToUInt16(_Bytes[index], 0);
        }

        public int ReadInt(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToInt32(_Bytes[index], 0);
        }

        public uint ReadUInt(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToUInt32(_Bytes[index], 0);
        }

        public long ReadLong(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToInt64(_Bytes[index], 0);
        }

        public ulong ReadULong(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToUInt64(_Bytes[index], 0);
        }

        public float ReadFloat(int index = -1)
        {
            index = Advance(index);
            return BitConverter.ToSingle(_Bytes[index], 0);
        }

        public double ReadDouble(int index = -1)
        {
            index = Advance(index);

            return BitConverter.ToDouble(_Bytes[index], 0);
        }

        public DateTime ReadDateTime(int index = -1)
        {
            index = Advance(index);
            return FromBytesDateTime(_Bytes[index], 0);
        }

        public string ReadString(int index = -1)
        {
            index = Advance(index);

            byte[] bits = _Bytes[index];
            return BytesToString(bits);
        }

        public Guid ReadGuid(int index = -1)
        {
            index = Advance(index);
            return new Guid(_Bytes[index]);
        }

        public byte[] Read(int index = -1)
        {
            index = Advance(index);
            return _Bytes[index];
        }

        private int Advance(int index)
        {
            bool ret = false;
            if (index == -1)
            {
                ret = true;
                index = _ActiveIndex;
            }

            if (ret)
                _ActiveIndex++;

            return index;
        }

        #endregion

        #region Read Items

        public bool[] ReadBools(int index = -1)
        {
            index = Advance(index);
            return ReadItems(1, BitConverter.ToBoolean, index);
        }

        public short[] ReadShorts(int index = -1)
        {
            index = Advance(index);
            return ReadItems(2, BitConverter.ToInt16, index);
        }

        public ushort[] ReadUShorts(int index = -1)
        {
            index = Advance(index);
            return ReadItems(2, BitConverter.ToUInt16, index);
        }

        public int[] ReadInts(int index = -1)
        {
            index = Advance(index);
            return ReadItems(_SI, BitConverter.ToInt32, index);
        }

        public uint[] ReadUInts(int index = -1)
        {
            index = Advance(index);
            return ReadItems(_SI, BitConverter.ToUInt32, index);
        }

        public long[] ReadLongs(int index = -1)
        {
            index = Advance(index);
            return ReadItems(8, BitConverter.ToInt64, index);
        }

        public ulong[] ReadULongs(int index = -1)
        {
            index = Advance(index);
            return ReadItems(8, BitConverter.ToUInt64, index);
        }

        public float[] ReadFloats(int index = -1)
        {
            index = Advance(index);
            return ReadItems(_SF, BitConverter.ToSingle, index);
        }

        public double[] ReadDoubles(int index = -1)
        {
            index = Advance(index);
            return ReadItems(_SD, BitConverter.ToDouble, index);
        }

        public Guid[] ReadGuids(int index = -1)
        {
            index = Advance(index);
            return ReadItems(16,
                (b, k) => new Guid(b.Skip(k).Take(16).ToArray()), index);
        }

        public DateTime[] ReadDateTimes(int index = -1)
        {
            index = Advance(index);
            return ReadItems(_SL, FromBytesDateTime, index);

            //byte[] bytes = _Bytes[index];
            //int n = bytes.Length / _SD;
            //DateTime[] ret = new DateTime[n];
            //int k = 0;
            //for (int i = 0; i < n; i++)
            //{
            //    ret[i] = DateTimeFromByteArray(bytes, k);
            //    k += _SD;
            //}
            //return ret;
        }

        public TimeSpan[] ReadTimeSpans(int index = -1)
        {
            index = Advance(index);
            return ReadItems(_SL, FromBytesTimeSpan, index);
        }

        public T[] ReadItems<T>(int stride,
            Func<byte[], int, T> toItem, int index = -1)
        {
            index = Advance(index);

            byte[] bits = _Bytes[index];

            int s = stride;
            int n = bits.Length / s;
            T[] ret = new T[n];

            int i = 0;
            for (int k = 0; k < n; k++)
            {
                ret[k] = toItem(bits, i);
                i += s;
            }

            return ret;
        }

        #endregion

        #region Static Helpers

        public static byte[] StringToBytes(string value)
        {
            if (value == null)
                return new byte[1];

            int s = _SC;
            int tot = value.Length * s;
            byte[] bits = new byte[tot + 1];
            bits[0] = 1;

            int i = 1;
            for (int k = 0; k < value.Length; k++)
            {
                Array.Copy(BitConverter.GetBytes(value[k]), 0, bits, i, s);
                i += s;
            }

            return bits;
        }

        public static string BytesToString(byte[] bits)
        {
            if (bits[0] == 0)
                return null;

            string ret = "";
            int s = _SC;
            int i = 1;
            for (int k = 0; k < bits.Length / s; k++)
            {
                ret += BitConverter.ToChar(bits, i);
                i += s;
            }

            return ret;
        }

        #endregion
    }
}
