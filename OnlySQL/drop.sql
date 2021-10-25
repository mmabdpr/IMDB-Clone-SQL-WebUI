drop view public.leaf_category_ids;

drop trigger passwd_trigger on public.user;
drop function passwd_func();

drop trigger payment_type_check_trigger on public.real_estate_info;
drop function payment_type_check_func();

drop trigger category_trigger on public.post;
drop function category_func();

drop table public.car_info;
drop table public.motorcycle_info;
drop table public.cloth_info;
drop table public.shoe_info;
drop table public.bag_info;
drop table public.real_estate_info;
drop table public.post_contact_info;
drop table public.user_star_post;
drop table public.post;
drop table public.category;
drop table public.user;
drop extension pgcrypto;

drop function get_category_lower_boundary(cat varchar);
drop function get_category_lower_boundary(cat_id int);
drop function get_category_upper_boundary(cat varchar);
drop function get_category_upper_boundary(cat_id int);

drop function f19(term varchar);
drop function f16(cat varchar);
drop function f17(cat varchar);