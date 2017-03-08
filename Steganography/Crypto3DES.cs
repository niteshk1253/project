using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Security.Cryptography; 

namespace Steganography
{
	//Created a class for triple DES Encryption & Decryption
    class Crypto3DES 
    { 
    	//This is an encryption method which has a parameter message and a key 
        public static byte[] 3DES_Encrypt(string msg,string key) 
        {
        	//Object of TripleDES is created and CreateDES(key) is invoked 
            TripleDES des = CreateDES(key);  
            //e_ict object is created to store des encryptor 
            ICryptoTransform e_ict = des.CreateEncryptor();  
            //This is used to store the message bytes after encoding  
            byte[] input = Encoding.Unicode.GetBytes(msg); 
            //This is used to hold the final transformation block with the input length 
            return e_ict.TransformFinalBlock(input,0,input.Length); 

        } 
        //This is an decryption method which has a parameter cipher_text and a key 
        public static string 3DES_Decrypt(string cipher_txt,string key) 
        {
        	//b is used to store the converted cipher_txt  
            byte[] b=Convert.FromBase64String(cipher_txt); 
            //Object of TripleDES is created and CreateDES(key) is invoked  
            TripleDES des=CreateDES(key); 
            //e_ict object is created to store des encryptor  
            ICryptoTransform d_ict=des.CreateDecryptor(); 
            //This is used to hold the final transformation block with the length of b  
            byte[] output=d_ict.TransformFinalBlock(b,0,b.Length); 
            //Output string is returned 
            return Encoding.Unicode.GetString(output); 
        } 
        //This method is of the class type TripleDES and has a parameter key  
        static TripleDES CreateDES(string key) 
        { 
        	//md5 object of MD5 is used to store new MD5CryptoServiceProvide()  
            MD5 md5=new MD5CryptoServiceProvider();  
            //new TripleDESCryptoServiceProvider() is used and stored in tdes  
            TripleDES tdes=new TripleDESCryptoServiceProvider(); 
            //Key is computed using ComputeHash method and stored in tdes.Key  
            tdes.Key=md5.ComputeHash(Encoding.Unicode.GetBytes(key)); 
            //new byte value is stored in tdes.IV  
            tdes.IV=new byte[tdes.BlockSize/8]; 
            //tdes is returned which will go to 3DES_Encrypt(string,string) and 3DES_Decrypt(string,string) method  
            return tdes; 
        } 
    } 
} 
