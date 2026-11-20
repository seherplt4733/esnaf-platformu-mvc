# Esnaf Platformu Proje Planı

## Amaç

Küçük işletmelerin kendi tanıtım sitelerini oluşturup yayınlayabileceği
bir ASP.NET Core MVC uygulaması geliştirmek.

İlk örnek işletme Limon & Kekik adlı bir restoran.
İlk site adresi /site/limon-ve-kekik.

## Ekran akışı

Kayıt → İşletme bilgileri → Menü → Tema seçimi → Önizleme → Yayın

- İşletme sahibi profilini ve menüsünü düzenler, önizler ve yayınlar.
- Ziyaretçi yayınlanmış işletme bilgilerini ve menüyü görür.
- Ada ve Çizgi temaları aynı işletme verisini kullanır.

## Taslak ve yayın

- İlk yayından önce ziyaretçi adresi 404 döndürür.
- İlk yayında Limonlu tavuk 280 TL görünür.
- Sahibi fiyatı 300 TL yapıp kaydedince önizleme 300 TL gösterir.
  Ziyaretçi ve herkese açık API son yayınlanan fiyatı, 280 TL'yi görür.
- İşletme sahibi yeniden yayınlayınca ziyaretçi ve API 300 TL'yi görür.

## API

Planlanan GET /api/v1/sites/limon-ve-kekik isteği,
yayınlanmış işletme bilgilerini ve menüyü JSON olarak döndürür.

## Geliştirme

- Gün 1: Ekran akışını ve yayın kurallarını belirle.
- Gün 2: MVC projesini oluştur, yerelde çalıştır ve GitHub'a gönder.
