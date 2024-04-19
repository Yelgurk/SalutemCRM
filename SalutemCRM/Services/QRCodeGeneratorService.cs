using Avalonia.Media;
using Avalonia.Media.Imaging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public partial class QRCodeGeneratorService
{
    private QRCodeGenerator _qrGenerator { get; init; }

    public QRCodeGeneratorService() => _qrGenerator = new QRCodeGenerator();

    public Bitmap Generate(string context) =>
        _qrGenerator.CreateQrCode(context, QRCodeGenerator.ECCLevel.Q)
        .Do(x => new PngByteQRCode(x))
        .Do(x => x.GetGraphic(16, new byte[4] { 0, 0, 0, 255 }, new byte[4] { 255, 255, 255, 0 }))
        .Do(x => {
            using (MemoryStream ms = new MemoryStream(x))
                return new Bitmap(ms);
        });
}
