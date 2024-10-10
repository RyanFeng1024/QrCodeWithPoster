# QrCodeWithPoster
使用**前端**和**后端**两种方式动态生成带海报的二维码.  
Dynamically generate a QR code with a poster or background image in both front-end and back-end methods..

## 纯前端生成  Front-end generation
`index.html` 为H5前端利用Canvas把两张图合并成一张，动态生成二维码海报。  
`index.html` uses Canvas to merge two images into one to dynamically generate a QR code poster

## 后端生成  Back-end generation
`QRCode.ashx.cs` 为使用C#后端代码动态生成海报二维码.  
`QRCode.ashx.cs` is used to dynamically generate poster QR codes using C# backend code.
```html
使用方法：  
<img src="/Service/QRCode.ashx?text=要生成二维码的内容&size=二维码尺寸&contentImg=是否包含背景图（true/false）" />

e.g.:  <img src="/Service/QRCode.ashx?text=https://www.baidu.com&size=160&contentImg=true" />
```
