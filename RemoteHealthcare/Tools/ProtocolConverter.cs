﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Tools
{
    class ProtocolConverter
    {
        //Converts a bytearray to a string, this can be used for displaying the contents of the array.
        public static String ByteArrayToString(byte[] array)
        {
            String toReturn = "";

            foreach (var name in array)
            {
                toReturn += name;
            }

            return toReturn;
        }

        public static byte PageChecker(byte[] payload)
        {
            //Filter on byte array size, should be redundant
            //if (payload.Length != 8) return (byte)0xFF;
            return payload[0];
        }

        //@deprecated
        //TODO make method so we can give multiple parameters for pagenumber and payloadnumber 
        public static byte ShowValue(byte[] payload)
        {
            byte pagenumber = PageChecker(payload);

            if (pagenumber == 16)
            {
                //Console.WriteLine(" {0}", payload[3]);
                Console.WriteLine("Speed: {0} km/h", (double)CombineBits(payload[5], payload[4]) * 0.001 * 3.6);
                return payload[3];
            }

            if (pagenumber == 25)
            {
                //Console.WriteLine("Rpm: {0}", payload[2]);
                return payload[2];
            }

            return (byte)0xFF;
        }

        //TO BE TESTED
        public static int ReadDataSet(byte[] payload, byte targetPageNumber, bool mustCombine, params byte[] targetByte)
        {
            //Check if we're reading the correct page
            byte pageNumberReceived = PageChecker(payload);
            if(pageNumberReceived == targetPageNumber)
            {
                //received bits to combine
                if(mustCombine && targetByte.Length == 2)  return CombineBits(payload[targetByte[0]], payload[targetByte[1]]);

                //received one bit and returns payload contents
                if (targetByte.Length == 1) return payload[targetByte[0]];

            }
            Console.WriteLine("Could not read dataset {0} from page {1} with first targetbyte {2}", payload, targetPageNumber, targetByte[0]);
            return -1;
        }

        //Data size is 13, this is the full dataset received by the device
        //Payload size is 8, contains the information obtained by the sensor
        public static byte[] DataToPayload(byte[] data)
        {
            if (data.Length != 13) return new byte[8];

            byte[] toReturn = new byte[8];

            for (int i = 4; i < 12; i++)
            {
                toReturn[i - 4] = data[i];
            }
            return toReturn;
        }

        public static ushort CombineBits(byte byte1, byte byte2)
        {
            //Bit shift first bit 8 to the left
            ushort combined = byte1;
            combined = (ushort)(combined << 8);

            //Or both bytes
            return (ushort)(combined | byte2);
        }

    }
}
