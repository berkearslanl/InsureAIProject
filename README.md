# 🛡️ InsureYouAI & Management System

Bu proje, modern bir sigorta yönetim sisteminin **.NET Core Enterprise** mimarisiyle inşa edilip, güncel **Yapay Zeka (AI)** modelleriyle (OpenAI, Gemini, Claude, ElevenLabs, Tavily) harmonize edilmiş ileri seviye bir versiyonudur.

Proje kapsamında sadece CRUD işlemleri değil; içerik üretimi, veri analitiği, sesli asistan ve tahminleme modelleri entegre edilmiştir.

## 🚀 Öne Çıkan Özellikler

### 🤖 Yapay Zeka Entegrasyonları
- **OpenAI:** Otomatik makale oluşturma, DALL-E ile kapak görseli üretimi ve SignalR tabanlı streaming chat asistanı.
- **Google Gemini:** Hakkımızda metinleri, mesaj kategorizasyonu ve akıllı öncelik belirleme sistemi.
- **Claude AI:** Müşteri yorum analizi, otomatik e-posta yanıtlama sistemi ve poliçe doküman analizi.
- **Hugging Face (Helsinki NLP):** Yorumlar için TR-EN dil çevirisi ve toksik içerik kontrolü.
- **ElevenLabs:** Metinden sese (Text-to-Speech) dönüşüm ile sesli asistan deneyimi.
- **Tavily AI:** Gerçek zamanlı internet araması tabanlı içerik zenginleştirme.

### 📊 Analiz ve Tahminleme
- **ML.NET Time Series:** Geçmiş verileri kullanarak gelecek dönem poliçe satış tahminleri (Forecasting).
- **Dinamik Dashboard:** OpenAI tabanlı akıllı grafikler (Revenue/Expense), anomali tespiti ve aylık trend analizleri.

### 🛠️ Teknik Katmanlar
- **Backend:** .NET Core 8.0 / 9.0, Entity Framework Core, Repository Pattern.
- **Frontend:** ASP.NET Core MVC, View Components, SignalR (Real-time).
- **Güvenlik:** ASP.NET Core Identity ile rol bazlı yetkilendirme.
- **Veritabanı:** SQL Server (Complex Queries, Sales Trends).

## 📸 Ekran Görüntüleri
| Ana Sayfa | Blog Detayı |
|---|---|
|<img width="1920" height="10973" alt="screencapture-localhost-7155-Default-Index-2026-02-16-14_49_32" src="https://github.com/user-attachments/assets/b04e335c-0cea-46b5-9b59-0e87a5ef44ba" /> | <img width="1920" height="5146" alt="screencapture-localhost-7155-Blog-BlogDetail-2-2026-02-16-15_05_09" src="https://github.com/user-attachments/assets/73f1c4af-6eea-42e3-beba-2007bf918a01" /> |

| Dashboard | Dashboard Tahminleme |
|--|--|
|<img width="1898" height="905" alt="Ekran görüntüsü 2026-02-16 150658" src="https://github.com/user-attachments/assets/1a430434-2246-4633-9966-156f7946d572" /> | <img width="1894" height="904" alt="Ekran görüntüsü 2026-02-16 150712" src="https://github.com/user-attachments/assets/1d339172-0970-42de-88ee-bb1b8e9dca36" /> |

| Poliçe Satış Tahminleme | Pdf Analizi |
|---|---|
|<img width="1916" height="906" alt="Ekran görüntüsü 2026-02-16 151202" src="https://github.com/user-attachments/assets/ef5ba0b2-cdb9-4ad5-97e2-a77c731d068d" /> | <img width="1894" height="904" alt="Ekran görüntüsü 2026-02-16 151255" src="https://github.com/user-attachments/assets/dd9cd2cf-8b35-427f-af36-da245b4f2e56" /> |

| Web'de Arama | Görsel Oluşturucu |
|---|---|
|<img width="1896" height="907" alt="Ekran görüntüsü 2026-02-16 151516" src="https://github.com/user-attachments/assets/2f3aed84-71d5-4fb9-8bc6-665ffff1988b" /> | <img width="1900" height="903" alt="Ekran görüntüsü 2026-02-16 151643" src="https://github.com/user-attachments/assets/0f62db54-950d-4ee9-bf79-22af6f4bcf0f" /> |

| Makale Oluşturucu | Referans Oluşturucu |
|---|---|
| <img width="1895" height="904" alt="Ekran görüntüsü 2026-02-16 151745" src="https://github.com/user-attachments/assets/a70502b1-aafb-46fc-bdae-68b1b2ef2635" /> | <img width="1915" height="903" alt="Ekran görüntüsü 2026-02-16 152012" src="https://github.com/user-attachments/assets/62d6717d-887d-4058-87e5-930a3381feb0" /> |

| Hakkımızda Taslağı | Hakkımızda Maddeleri |
|---|---|
| <img width="1895" height="900" alt="Ekran görüntüsü 2026-02-16 151903" src="https://github.com/user-attachments/assets/591248fe-5bac-45f1-8073-ebe973518273" /> | <img width="1894" height="903" alt="Ekran görüntüsü 2026-02-16 151926" src="https://github.com/user-attachments/assets/71f7eb9b-f4b5-4216-8cd0-6fa4d1b3b1d5" /> |

| ChatBot | Ödeme Planı Önerme |
|---|---|
| <img width="1913" height="903" alt="Ekran görüntüsü 2026-02-16 161850" src="https://github.com/user-attachments/assets/b46c806b-ee6c-4a6d-a521-da8cb07c0407" /> | <img width="1896" height="904" alt="Ekran görüntüsü 2026-02-16 152051" src="https://github.com/user-attachments/assets/0b3b9fbb-dfc0-41ec-bb76-27d9b9777fdc" /> |

| Mesaj Detayı | Mesaja Otomatik Mail Dönüşü |
|---|---|
| <img width="1895" height="906" alt="Ekran görüntüsü 2026-02-16 152140" src="https://github.com/user-attachments/assets/ec9adb8c-a989-48a6-9bea-14103cff8fb1" /> | <img width="1440" height="513" alt="Ekran görüntüsü 2026-02-16 152304" src="https://github.com/user-attachments/assets/d8e4ccff-1049-429b-91f0-cb79059ec986" /> |




