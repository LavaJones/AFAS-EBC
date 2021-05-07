using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;
using System.IO;
using System.Security.Cryptography;

public class SecurityHeader
{

    public int employer_id { get; set; }

    public Guid ResourceId { get; set; }

    public String OriginalReceiptId { get; set; }

    public String TimestampDigestValue { get { return Transmitter.TimestampDigestValue; } }

    public String ACABusinessHeaderDigestValue { get { return Transmitter.ACABusinessHeaderDigestValue; } }

    public String ACABulkRequestTransmitterStatusDetailRequestDigestValue { get { return Transmitter.ACABulkRequestTransmitterStatusDetailRequestDigestValue; } }

    public String KeyIdentifier { get { return Transmitter.KeyIdentifier; } }

    public String TimestampCreated { get; private set; }

    public String TimestampExpires { get; private set; }

    public String PaymentYr { get; set; }

    public String PriorYearDataInd { get; set; }

    public String TransmissionTypeCd { get; set; }

    public String TestFileCd { get { return Transmitter.TestFileCd; } }

    public String EIN { get { return Transmitter.EIN; } }

    public String BusinessNameLine1Txt { get { return Transmitter.BusinessNameLine1; } }

    public String BusinessNameLine2Txt { get { return String.Empty; } }

    public String CompanyNm { get { return Branding.CompanyName; } }

    public String AddressLine1Txt { get { return Branding.AddressStreet; } }

    public String CityNm { get { return Transmitter.City; } }

    public String USStateCd { get { return Transmitter.State; } }

    public String USZIPCd { get { return Transmitter.Zip; } }

    public String USZIPExtensionCd { get { return String.Empty; } }

    public String PersonFirstNm { get { return Transmitter.PersonFirstNm; } }

    public String PersonLastNm { get { return Transmitter.PersonLastNm; } }

    public String ContactPhoneNum { get { return Transmitter.ContactPhoneNum; } }

    public String VendorCd { get { return Transmitter.VendorCd; } }

    public String VendorPersonFirstNm { get { return Transmitter.VendorPersonFirstNm; } }

    public String VendorPersonLastNm { get { return Transmitter.VendorPersonLastNm; } }

    public String VendorContactPhoneNum { get { return Transmitter.VendorContactPhoneNum; } }

    public String TotalPayeeRecordCnt { get; private set; }

    public String TotalPayerRecordCnt { get; private set; }

    public String SoftwareId { get { return Transmitter.SoftwareId; } }

    public String FormTypeCd { get { return "1094/1095C"; } }

    public String BinaryFormatCd { get { return "application/xml"; } }

    public String ChecksumAugmentationNum { get; private set; }

    public String AttachmentByteSizeNum { get; private set; }

    public String DocumentSystemFileNm { get; private set; }

    public String UniqueTransmissionId { get { return string.Format("{0}:SYS12:{1}::T", ResourceId.ToString(), Transmitter.TransmitterControlCode); } }

    public String Timestamp { get; private set; }

    public String FullTimestamp { get; private set; }

    public List<Form1094CUpstreamDetail> Form1094CUpstreamDetails { get; set; }

    public void StartTimestamp()
    {
        DateTime now = DateTime.Now;
        this.TimestampCreated = now.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    public void EndTimestampAndSetFileName()
    {
        DateTime now = DateTime.Now;
        this.Timestamp = now.ToString("yyyy-MM-ddTHH:mm:ss");
        this.TimestampExpires = this.Timestamp;
        this.FullTimestamp = now.ToString("yyyyMMddTHHmmssfffZ");
        this.DocumentSystemFileNm = string.Format("1094C_Request_{0}_{1}.xml", Transmitter.TransmitterControlCode, this.FullTimestamp);
    }

    public void SetCountValues()
    {
        var totalPayeeRecordCnt = 0;
        foreach (var form1094CUpstreamDetail in this.Form1094CUpstreamDetails)
        {
            int form1095CAttachedCnt = 0;
            if(int.TryParse(form1094CUpstreamDetail.Form1095CAttachedCnt, out form1095CAttachedCnt))
                totalPayeeRecordCnt += form1095CAttachedCnt;

        }
        this.TotalPayeeRecordCnt = totalPayeeRecordCnt.ToString();
        this.TotalPayerRecordCnt = this.Form1094CUpstreamDetails.Count.ToString();
    }

    public void SetAttachmentByteSizeNumAndChecksumAugmentationNum(String filename)
    {
        var formBytes = File.ReadAllBytes(filename);
        this.AttachmentByteSizeNum = formBytes.Count().ToString();
        var checkSum = BitConverter.ToString(SHA256Managed.Create().ComputeHash(formBytes)).Replace("-", "").ToLower();
        this.ChecksumAugmentationNum = checkSum;
    }

}