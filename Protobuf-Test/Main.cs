using System;
using System.IO;
using System.Collections.Generic;
using ProtoBuf;


namespace ProtobufTest
{
    /**
     * Test Protobuf-net v2 with complex types
     *
     */

    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MemoryStream ms = new MemoryStream();

            Person p = new Person() {
                id = int.MaxValue
                , tFloat = 13141.023f
                , name = "Ben"
            };

            p.likedThings.Add("coffee"  , 100);
            p.likedThings.Add("cookies" , 599);
            p.likedThings.Add("veggies" , -100);

            p.myLikes.Add(new LikedThing("one", 1));
            p.myLikes.Add(new LikedThing("two", 2));

            p.randomData.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("abcde"));

            Serializer.Serialize(ms, p);
            byte[] bin = ms.ToArray();
            Console.WriteLine(string.Format("Size: {0}, bytes: {1}", bin.Length, BitConverter.ToString(bin)));

            Person p2 = Serializer.Deserialize<Person>(new MemoryStream(bin));
            Console.WriteLine(p2);
        }
    }

    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public int id = 0;

        [ProtoMember(2)]
        public string name;

        [ProtoMember(3)]
        public float tFloat;

        [ProtoMember(4)]
        public double tDouble;

        [ProtoMember(5)]
        public Dictionary<string, int> likedThings =  new Dictionary<string, int>();

        [ProtoMember(6)]
        public List<byte> randomData = new List<byte>();

        [ProtoMember(7)]

        public List<LikedThing> myLikes = new List<LikedThing>();

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("{0} : {1}\n", this.id, this.name));
            foreach (KeyValuePair<string, int> kvp in likedThings)
            {
                sb.Append(string.Format(" - Likes: {0} x {1}\n", kvp.Key, kvp.Value));
            }

            sb.Append("Byte Array: " + System.Text.ASCIIEncoding.ASCII.GetString(randomData.ToArray()) + "\n");

            foreach (LikedThing lt in myLikes)
            {
                sb.Append(string.Format(" - like2: {0} => {1}\n", lt.name, lt.vote));
            }

            return sb.ToString();
        }
    }

    [ProtoContract]
    public class LikedThing
    {
        [ProtoMember(1)]
        public string name;

        [ProtoMember(2)]
        public int vote;


        public LikedThing()
        {
        }

        public LikedThing(string name, int vote)
        {
            this.name = name;
            this.vote = vote;
        }

    }


}

