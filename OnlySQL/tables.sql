create table public.user
(
    UserId      serial primary key,
    Email       varchar not null,
    FirstName   varchar not null,
    LastName    varchar not null,
    PhoneNumber int     not null,
    Password    text    not null
);

create table public.category
(
    CategoryId int primary key,
    Title      varchar not null,
    Lft        int     not null,
    Rgt        int     not null
);

create table public.post
(
    PostId      serial primary key,
    Image       text, -- image file path
    Title       varchar   not null,
    Description text,
    CategoryId  int       not null,
    DateTime    timestamp not null,
    CreatorId   int       not null,
    City        varchar   not null,
    SeenCount   int       not null default (0),
    TotalPrice  bigint,
    foreign key (CategoryId) references category (CategoryId),
    foreign key (CreatorId) references public.user (UserId)
);

create table public.user_star_post
(
    UserId int not null,
    PostId int not null,
    foreign key (UserId) references public.user (UserId),
    foreign key (PostId) references public.post (PostId)
);

create table public.post_contact_info
(
    PostId            int primary key,
    PhoneNumber       int not null,
    Email             varchar,
    Address           varchar,
    foreign key (PostId) references public.post (PostId)
);

create table public.real_estate_info
(
    PostId           int,
    PaymentType      int, -- buy = 0, rent = 1
    MonthlyPrice     int,
    PrePayPrice      int,
    ConstructionYear int,
    Area             int,
    RoomCount        int,
    Floor            int,
    HasElevator      bool,
    HasParking       bool,
    HasStoreroom     bool,
    LocationLat      float,
    LocationLong     float,
    foreign key (PostId) references public.post (PostId)
);

create table public.car_info
(
    PostId         int,
    Brand          varchar not null,
    ProductionYear int     not null,
    Usage          int     not null,
    BodyStatus     varchar default ('بدون رنگ'),
    Color          varchar not null,
    GearBox        bool, -- auto = f, manual = t
    foreign key (PostId) references public.post (PostId)
);

create table public.motorcycle_info
(
    PostId         int,
    Brand          varchar not null,
    ProductionYear int     not null,
    Usage          int     not null,
    foreign key (PostId) references public.post (PostId)

);

create table public.cloth_info
(
    PostId            int,
    Brand             varchar,
    Status            varchar default ('در حد نو'),
    Type              varchar default ('سایر'),
    Gender            bool not null, -- m : t, f: f
    ProductionCountry varchar,
    Size              varchar,
    foreign key (PostId) references public.post (PostId)

);

create table public.shoe_info
(
    PostId int,
    Brand  varchar,
    Gender bool,
    Size   int,
    foreign key (PostId) references public.post (PostId)
);

create table public.bag_info
(
    PostId      int,
    Brand       varchar,
    Type        varchar,
    WeightGrams int,
    Material    varchar,
    foreign key (PostId) references public.post (PostId)
);

create extension pgcrypto;