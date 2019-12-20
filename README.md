# QrCodeWithPoster
动态生成带海报的二维码. <https://myron1024.github.io/QrCodeWithPoster/>

```
index.html 为H5前端利用Canvas把两张图合并成一张，动态生成二维码海报
```
```
QRCode.ashx.cs 为利用C#后端动态生成海报二维码.
  使用方法： <img src="/Service/QRCode.ashx?text=要生成二维码的内容&size=二维码尺寸&contentImg=是否包含背景图（true/false）" />
  eg: <img src="/Service/QRCode.ashx?text=https://www.baidu.com&size=160&contentImg=true" />
```
