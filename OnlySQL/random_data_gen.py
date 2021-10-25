import os
import random
from faker import Faker
import codecs

fake = Faker()


def truncate_utf8_chars(filename, count, ignore_newlines=True):
    with open(filename, 'rb+') as f:
        last_char = None

        size = os.fstat(f.fileno()).st_size

        offset = 1
        chars = 0
        while offset <= size:
            f.seek(-offset, os.SEEK_END)
            b = ord(f.read(1))

            if ignore_newlines:
                if b == 0x0D or b == 0x0A:
                    offset += 1
                    continue

            if b & 0b10000000 == 0 or b & 0b11000000 == 0b11000000:
                chars += 1
                if chars == count:
                    f.seek(-1, os.SEEK_CUR)
                    f.truncate()
                    return
            offset += 1


class DataGen():
    def __init__(self):
        self.categories = [3, 4, 5, 7, 8, 10, 11]
        self.categories_price_range = [(50_000, 3_000_000), (100_000, 5_000_000), (60_000, 3_000_000), (
            2_000_000, 150_000_000), (50_000_000, 900_000_000), (1_000_000_000, 100_000_000_000), (300_000_000, 1_000_000_000)]
        self.categories_titles = ['لباس', 'کفش', 'کیف',
                                  'موتورسیکلت', 'اتومبیل', 'خانه', 'دفتر']
        self.categories_tables = [self.cloth_info, self.shoe_info, self.bag_info,
                                  self.motorcycle_info, self.car_info, self.real_estate_info, self.real_estate_info]
        self.users = [i for i in range(1, 10)]
        self.cities = ['تهران', 'کرج', 'یزد', 'مشهد', 'شیراز', 'تبریز']

    def real_estate_info(self, post_id):
        with codecs.open('pre/real_estate_info.sql', 'a', "utf-8") as real_estate_f:
            pt = random.choice([0, 1])
            mp = 'null'
            pp = 'null'
            if pt == 1:
                mp = random.randint(10, 500) * 100_000
                pp = random.randint(200, 8000) * 100_000
            cy = random.randint(1370, 1399)
            ar = random.randint(40, 5_000)
            rc = random.randint(1, 6)
            fl = random.randint(1, 20)
            he = random.choice(['true', 'false'])
            hp = random.choice(['true', 'false'])
            hs = random.choice(['true', 'false'])
            ll = random.randint(35620000, 35786600) / 1_000_000
            lg = random.randint(51260000, 51513200) / 1_000_000
            print(
                f"({post_id}, {pt}, {mp}, {pp}, {cy}, {ar}, {rc}, {fl}, {he}, {hp}, {hs}, {ll}, {lg}),\n", end='')
            real_estate_f.write(
                f"({post_id}, {pt}, {mp}, {pp}, {cy}, {ar}, {rc}, {fl}, {he}, {hp}, {hs}, {ll}, {lg}),\n")

    def car_info(self, post_id):
        with codecs.open('pre/car_info.sql', 'a', "utf-8") as car_f:
            br = f'car brand {random.choice([1, 2, 3, 4, 5])}'
            py = random.randint(1380, 1399)
            us = random.randint(10, 150) * 1000
            bs = random.choice(
                ['کاملا سالم', 'بدون رنگ', 'یک لکه رنگ', 'دو لکه رنگ', 'بدنه تعویض', 'کاپوت تعویض'])
            co = random.choice(
                ['مشکی', 'سفید', 'آبی', 'قرمز', 'بژ', 'خاکستری', 'نقره ای'])
            gb = random.choice(['true', 'false'])
            print(f"({post_id},'{br}',{py},{us},'{bs}','{co}',{gb}),\n", end='')
            car_f.write(f"({post_id},'{br}',{py},{us},'{bs}','{co}',{gb}),\n")

    def motorcycle_info(self, post_id):
        with codecs.open('pre/motorcycle_info.sql', 'a', "utf-8") as moto_f:
            br = f'moto brand {random.choice([1, 2, 3, 4, 5])}'
            py = random.randint(1380, 1399)
            us = random.randint(5, 60) * 1000
            print(f"({post_id}, '{br}', {py}, {us}),\n", end='')
            moto_f.write(f"({post_id}, '{br}', {py}, {us}),\n")

    def cloth_info(self, post_id):
        with codecs.open('pre/cloth_info.sql', 'a', "utf-8") as cloth_f:
            brand = f'cloth brand {random.choice([1, 2, 3])}'
            st = random.choice(['در حد نو', 'کمی رنگ رفته', 'دوخت مجدد'])
            tp = random.choice(['تاپ', 'مجلسی', 'خواب', 'رسمی', 'اسپورت'])
            g = random.choice(['true', 'false'])
            pc = random.choice(['ایران', 'ترکیه', 'چین', 'امریکا'])
            s = random.choice(['xs', 's', 'm', 'l', 'xl'])
            print(
                f"({post_id}, '{brand}', '{st}', '{tp}', {g}, '{pc}', '{s}'),\n", end='')
            cloth_f.write(
                f"({post_id}, '{brand}', '{st}', '{tp}', {g}, '{pc}', '{s}'),\n")

    def shoe_info(self, post_id):
        with codecs.open('pre/shoe_info.sql', 'a', "utf-8") as shoe_f:
            brand = f'shoe brand {random.choice([1, 2, 3])}'
            g = random.choice(['true', 'false'])
            s = random.randint(32, 47)
            print(f"({post_id}, '{brand}', {g}, {s}),\n", end='')
            shoe_f.write(f"({post_id}, '{brand}', {g}, {s}),\n")

    def bag_info(self, post_id):
        with codecs.open('pre/bag_info.sql', 'a', "utf-8") as bag_f:
            brand = f'bag brand {random.choice([1, 2, 3])}'
            tp = random.choice(['کیف دستی', 'کوله پشتی', 'کیف پول'])
            w = random.randint(100, 2000)
            m = random.choice(['پلاستیکی', 'پارچه ای', 'چرمی'])
            print(f"({post_id}, '{brand}', '{tp}', {w}, '{m}'),\n", end='')
            bag_f.write(f"({post_id}, '{brand}', '{tp}', {w}, '{m}'),\n")

    def generate_info(self, category_id, id):
        self.categories_tables[self.categories.index(category_id)](id)

    def generate_contact_info(self, user_id, post_id):
        with codecs.open('pre/post_contact_info.sql', 'a', 'utf-8') as c_f:
            pn = f'0214466557{user_id}'
            em = random.choice([f"'user{user_id}@gmail.com'", 'null'])
            ad = random.choice([f"'address {user_id}'", 'null'])
            print(f"({post_id}, {pn}, {em}, {ad}),\n", end='')
            c_f.write(f"({post_id}, {pn}, {em}, {ad}),\n")

    def gen(self):
        post_count = 10000

        tables = ['real_estate_info',
                  'car_info',
                  'motorcycle_info',
                  'cloth_info',
                  'shoe_info',
                  'bag_info',
                  'post_contact_info',
                  'post',
                  'user_star_post']

        for t in tables:
            with codecs.open(f'pre/{t}.sql', 'w', 'utf-8') as f:
                f.write(f'insert into public.{t}\nvalues\n')

        with codecs.open("pre/post.sql", "a", "utf-8") as post_f:
            for i in range(1, post_count + 1):
                id = i
                image = random.choice([f"'img{id}.jpg'", 'null'])
                desc = f'description{id}'
                category_id = random.choice(self.categories)
                title = f'{self.categories_titles[self.categories.index(category_id)]}{id}'
                creator_id = random.choice(self.users)
                city = random.choice(self.cities)
                date_time = fake.date_time_between(
                    start_date='-5y', end_date='now').strftime('%Y-%m-%d %H:%M:%S+03:30')
                seen_count = random.randint(100, 100000)
                p = random.randint(self.categories_price_range[self.categories.index(
                    category_id)][0], self.categories_price_range[self.categories.index(category_id)][1]) // 1000 * 1000
                total_price = p if category_id not in (
                    10, 11) else random.choice([p, 'null'])

                print(f"({id}, {image}, '{title}', '{desc}', {category_id}, '{date_time}', {creator_id}, '{city}', {seen_count}, {total_price}),\n", end='')
                post_f.write(
                    f"({id}, {image}, '{title}', '{desc}', {category_id}, '{date_time}', {creator_id}, '{city}', {seen_count}, {total_price}),\n")

                self.generate_info(category_id, id)
                self.generate_contact_info(creator_id, id)

        with codecs.open("pre/user_star_post.sql", "a", "utf-8") as stars_f:
            for ui in range(1, 10):
                star_posts = random.sample(
                    [i for i in range(1, post_count + 1)], random.randint(20, post_count // 3 if i != 5 else 54))
                for spid in star_posts:
                    print(f"({ui}, {spid}),\n", end='')
                    stars_f.write(f"({ui}, {spid}),\n")


        for t in tables:
            truncate_utf8_chars(f'pre/{t}.sql', 2, False)
            with codecs.open(f'pre/{t}.sql', 'a', 'utf-8') as f:
                f.write(f';')

g = DataGen()
g.gen()
