create view public.leaf_category_ids
as
select CategoryId
from category
where Rgt = Lft + 1;