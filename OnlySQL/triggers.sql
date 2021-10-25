create function passwd_func()
    returns TRIGGER
as
$$
begin
    if length(NEW.Password) < 8 or NEW.Password is null then
        raise notice 'Pass: %', NEW.Password;
        raise exception 'password cannot be less than 8 characters';
    end if;
    NEW.Password := crypt(NEW.Password, gen_salt('bf', 8));
    return NEW;
end;
$$
    language plpgsql;

create trigger passwd_trigger
    before insert or update
    on public.user
    for each row
execute procedure passwd_func();

-------------------------------------------

create function payment_type_check_func()
    returns TRIGGER
as
$$
begin
    if NEW.PaymentType != 0 and NEW.PaymentType != 1 then
        raise exception 'Invalid PaymentType';
    end if;
    return NEW;
end;
$$
    language plpgsql;

create trigger payment_type_check_trigger
    before insert or update
    on public.real_estate_info
    for each row
execute procedure payment_type_check_func();

-------------------------------------------

create function category_func()
    returns TRIGGER
as
$$
begin
    if NEW.CategoryId not in (select * from public.leaf_category_ids) then
        raise exception 'Have to choose leaf category';
    end if;
    return NEW;
end;
$$
    language plpgsql;

create trigger category_trigger
    before insert or update
    on public.post
    for each row
execute procedure category_func();