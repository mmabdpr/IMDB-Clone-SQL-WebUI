create function get_category_lower_boundary(cat varchar) returns int as
$$
begin
    return (select c.lft
            from public.category c
            where c.title = cat);
end;
$$ LANGUAGE plpgsql;

create function get_category_lower_boundary(cat_id int) returns int as
$$
begin
    return (select c.lft
            from public.category c
            where c.categoryid = cat_id);
end;
$$ LANGUAGE plpgsql;

create function get_category_upper_boundary(cat varchar) returns int as
$$
begin
    return (select c.rgt
            from public.category c
            where c.title = cat);
end;
$$ LANGUAGE plpgsql;

create function get_category_upper_boundary(cat_id int) returns int as
$$
begin
    return (select c.rgt
            from public.category c
            where c.categoryid = cat_id);
end;
$$ LANGUAGE plpgsql;