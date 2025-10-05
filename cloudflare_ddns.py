import requests

# ===== CẤU HÌNH CỦA EM =====
ZONE_ID = '26cedf45e311cdd387ef740b3c97c0b1'       # Lấy từ Cloudflare dashboard
RECORD_ID = '20be7900f743f7b527a3b76f7eb761bf'   # Lấy từ API hoặc qua giao diện
API_TOKEN = 'MkWcWfoH04QmUFcfIdvmQtp-HRuOASpZY2H6tZjY'   # Token vừa tạo
DOMAIN = 'restx.food'

# ===== CODE BẮT ĐẦU =====
def get_ip():
    return requests.get('https://api.ipify.org').text

def update_ip(ip):
    url = f"https://api.cloudflare.com/client/v4/zones/{ZONE_ID}/dns_records/{RECORD_ID}"
    headers = {
        "Authorization": f"Bearer {API_TOKEN}",
        "Content-Type": "application/json"
    }
    data = {
        "type": "A",
        "name": DOMAIN,
        "content": ip,
        "ttl": 120,
        "proxied": False
    }
    res = requests.put(url, json=data, headers=headers)
    print(res.json())

ip = get_ip()
update_ip(ip)
