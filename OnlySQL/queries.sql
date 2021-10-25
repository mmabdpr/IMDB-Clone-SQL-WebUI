-- 1
select *
from public.user;

-- 2
select *
from public.user
where userid in (
    select distinct creatorid
    from public.post
);

-- 3
select *
from public.user u
where u.userid in (
    select creatorid
    from public.post
    where creatorid = u.UserId
      and image is not null
    group by creatorid
    having count(*) >= 2
);

-- 4
select *
from public.user u
where u.userid in (
    select creatorid
    from public.post
    where creatorid = u.UserId
    group by creatorid
    having sum(seencount) >= 100
);

-- 5
select *
from public.user u
where u.userid in (
    select s.userid
    from public.user_star_post s
    group by s.userid
    having count(*) > 10
);

-- 6
select *
from public.post p
where p.seencount > 1000;

-- 7
select *
from public.post p
where p.postid in (
    select s.postid
    from public.user_star_post s
    group by s.postid
    having count(*) > 100
);

-- 8
select *
from public.post p
where p.city = 'تهران';

-- 9
select *
from public.post p
order by p.seencount desc
limit 5;

-- 10
select sum(p.totalprice)
from public.post p
where p.city = 'کرج';

-- 11
select p.city
from public.post p
group by p.city
order by count(*) desc
limit 3;

-- 12
select p.city
from public.post p
where p.image is null
group by p.city
order by count(*) desc
limit 1;

-- 13
select *
from public.post p
where get_category_upper_boundary(p.categoryid) <= get_category_upper_boundary('خانه')
  and get_category_lower_boundary(p.categoryid) >= get_category_lower_boundary('خانه')
  and p.city = 'تهران';


-- 14
select *
from public.post p
         join public.real_estate_info rs
              on p.postid = rs.postid
where rs.paymenttype = 0
  and p.totalprice between 1000000000 and 3000000000
  and get_category_upper_boundary(p.categoryid) <= get_category_upper_boundary('خانه')
  and get_category_lower_boundary(p.categoryid) >= get_category_lower_boundary('خانه')
  and p.city = 'تهران';

-- 15
select sum(p.totalprice) * 10 as sum
from public.post p
where p.city = 'یزد'
  and get_category_lower_boundary('املاک') < get_category_lower_boundary(p.categoryid)
  and get_category_upper_boundary(p.categoryid) < get_category_upper_boundary('املاک');

-- 16
CREATE FUNCTION f16(cat varchar) RETURNS float AS
$$
DECLARE
    sum_star bigint default 1;
    sum_cat  bigint default 1;
BEGIN
    sum_star := (
        select sum(p.totalprice)
        from public.post p
        where get_category_lower_boundary(cat) < get_category_lower_boundary(p.categoryid)
          and get_category_upper_boundary(p.categoryid) < get_category_upper_boundary(cat)
          and p.postid in (
            select distinct postid
            from public.user_star_post));

    raise notice 'sum_star %', sum_star;

    sum_cat := (
        select sum(p.totalprice)
        from public.post p
        where get_category_lower_boundary(cat) < get_category_lower_boundary(p.categoryid)
          and get_category_upper_boundary(p.categoryid) < get_category_upper_boundary(cat));

    raise notice 'sum_cat %', sum_cat;

    RETURN (sum_star::decimal / sum_cat);
END;
$$ LANGUAGE plpgsql;

select f16('املاک'); -- example, can be changed to any category


-- 17
CREATE FUNCTION f17(cat varchar) RETURNS float AS
$$
DECLARE
    sum_img   bigint default 1;
    sum_n_img bigint default 1;
BEGIN
    sum_img := (
        select sum(p.totalprice)
        from public.post p
        where get_category_lower_boundary(cat) < get_category_lower_boundary(p.categoryid)
          and get_category_upper_boundary(p.categoryid) < get_category_upper_boundary(cat)
          and p.image is not null);

    raise notice 'sum_n_img %', sum_n_img;

    sum_n_img := (
        select sum(p.totalprice)
        from public.post p
        where get_category_lower_boundary(cat) < get_category_lower_boundary(p.categoryid)
          and get_category_upper_boundary(p.categoryid) < get_category_upper_boundary(cat)
          and p.image is null);

    raise notice 'sum_n_img %', sum_n_img;

    RETURN (sum_img::decimal / sum_n_img);
END;
$$ LANGUAGE plpgsql;

select f17('پوشیدنی'); -- example can be changed to any category

-- 18
select *
from public.post p
order by p.datetime desc;

-- 19
CREATE FUNCTION f19(term varchar)
    RETURNS table
            (
                PostId      int,
                Image       text,
                Title       varchar,
                Description text,
                CategoryId  int,
                DateTime    timestamp,
                CreatorId   int,
                City        varchar,
                SeenCount   int,
                TotalPrice  bigint
            )
AS
$$
BEGIN
    RETURN QUERY (select *
                  from public.post p
                  where p.title like CONCAT('%', term, '%'));
END;
$$ LANGUAGE plpgsql;

select *
from f19('لباس');


-- 20
select *
from public.post p
join public.car_info c
on p.postid = c.postid
where c.bodystatus = 'بدون رنگ'
  and p.totalprice between 100000000 and 300000000
order by p.totalprice desc;
