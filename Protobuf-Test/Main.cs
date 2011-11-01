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
            MemoryStream ms = new MemoryStream();

            Person p = new Person() {
                id = int.MaxValue
                , tFloat = 13141.023f
                , name = "Ben"
                , reallyBigInt = ulong.MaxValue
            };

            p.likedThings.Add("coffee"  , 100);
            p.likedThings.Add("cookies" , 599);
            p.likedThings.Add("veggies" , -100);

            p.myLikes.Add(new LikedThing("one", 1));
            p.myLikes.Add(new LikedThing("two", 2));

            p.randomData.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("abcde21300 9fa9j 12031"));

            Serializer.Serialize(ms, p);
            byte[] bin = ms.ToArray();
            Console.WriteLine(string.Format("Size: {0}\nbytes: {1}", bin.Length, BitConverter.ToString(bin)));

            Person p2 = Serializer.Deserialize<Person>(new MemoryStream(bin));
            Console.WriteLine(p2);
/*
Output:

Size: 137
bytes: 08-FF-FF-FF-FF-07-12-03-42-65-6E-1D ....
2147483647 : Ben
Big Number: 18446744073709551615
 - Likes: coffee x 100
 - Likes: cookies x 599
 - Likes: veggies x -100
Byte Array: abcde21300 9fa9j 12031
Other Likes
 - like2: one => 1
 - like2: two => 2
 
 */
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

        [ProtoMember(8)]
        public ulong reallyBigInt;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("{0} : {1}\n", this.id, this.name));
            sb.Append(string.Format("Big Number: {0}\n", reallyBigInt));
            foreach (KeyValuePair<string, int> kvp in likedThings)
            {
                sb.Append(string.Format(" - Likes: {0} x {1}\n", kvp.Key, kvp.Value));
            }

            sb.Append("Byte Array: " + System.Text.ASCIIEncoding.ASCII.GetString(randomData.ToArray()) + "\n");

            sb.Append("Other Likes\n");
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

