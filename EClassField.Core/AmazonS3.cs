using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public class AmazonS3
    {


        public static void UploadFile(Stream stream, string filename, string folder = "ilanimages/")
        {


            try
            {
                using (IAmazonS3 client = new AmazonS3Client())
                {


                    TransferUtility fileTransferUtility = new
                        TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.USEast1));




                    {
                        fileTransferUtility.Upload(stream,
                                                   "otonomide", folder + filename);
                    }
                    // do stuff
                }





            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                                  s3Exception.InnerException);
            }
        }

        public static Stream GetFile(string bucketname, string filename)
        {
            using (IAmazonS3 client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {

                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketname,
                    Key = filename
                };
                Stream stream;
                using (GetObjectResponse response = client.GetObject(request))
                {


                    stream = response.ResponseStream;




                }

                return stream;
            }

            return null;






        }

        public static void DeleteFile(string bucketname, string filename)
        {

            using (IAmazonS3 client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketname,
                    Key = "ilanimages/"+ filename
                };


             var a=   client.DeleteObject(deleteObjectRequest);
            }

        }

    }
}

