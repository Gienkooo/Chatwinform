using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_winform
{
    public enum ClientPackets
    {
        handshake,
        message,
    }
    public enum ServerPackets
    {
        handshake,
        message,
        refuseConnection,
        ping,
        hostMessage
    }
    public class Packet : IDisposable
    {
        List<byte> buffer;
        byte[] readableBuffer;
        Int32 readPos;
    
        public Packet()
        {
            buffer = new List<byte>();
            readPos = 0;
        }
    
        public Packet(Int32 _id) : this()
        {
            Write(_id);
        }
    
        public Packet(byte[] _value) : this()
        {
            readableBuffer = _value;
        }

        public Packet(Packet _value) : this()
        {
            readableBuffer = null;
            if(_value.UnreadLength() > 0)
            {
                SetBytes(_value.ReadBytes(_value.UnreadLength()));
            }
        }
    
        public Int32 Length()
        {
            return buffer.Count;
        }
    
        public Int32 UnreadLength()
        {
            return readableBuffer.Length - readPos;
        }
    
        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
            {
                buffer.Clear();
                readableBuffer = null;
                readPos = 0;
            }
            else
            {
                readPos -= sizeof(Int32);
            }
        }
    
        public byte[] ToArray()
        {
            return buffer.ToArray();
        }
    
        public void SetBytes(byte[] _data)
        {
            Write(_data);
            readableBuffer = buffer.ToArray();
        }
    
        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        public byte ReadByte(bool _moveReadPos = true)
        {
            if (UnreadLength() > 0)
            {
                byte value = readableBuffer[readPos];
                if (_moveReadPos)
                {
                    readPos += sizeof(byte);
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read data of type 'byte'");
            }
        }
    
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (UnreadLength() > 0)
            {
                byte[] value = buffer.GetRange(readPos, _length).ToArray();
                if (_moveReadPos)
                {
                    readPos += _length;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read data of type 'byte[]'");
            }
        }
    
        public Int32 ReadInt32(bool _moveReadPos = true)
        {
            if (UnreadLength() > 0)
            {
                Int32 value = BitConverter.ToInt32(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += sizeof(Int32);
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read data of type 'Int32'");
            }
        }
    
        public String ReadString(bool _moveReadPos = true)
        {
            if (UnreadLength() > 0)
            {
                Int32 length = ReadInt32();
                Console.WriteLine($"Reading string of size: {length}");
                String value = System.Text.Encoding.ASCII.GetString(readableBuffer, readPos, length);
                if (_moveReadPos)
                {
                    readPos += length;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read data of type 'String'");
            }
        }

        public void Write(byte _value)
        {
            buffer.Add(_value);
        }
    
        public void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }
    
        public void Write(Int32 _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
    
        public void Write(String _value)
        {
            Write(_value.Length);
            Write(System.Text.Encoding.ASCII.GetBytes(_value));
        }

        private bool disposed = false;
    
        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }
                disposed = true;
            }
        }
    
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
