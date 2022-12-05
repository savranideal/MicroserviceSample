DO $$
BEGIN
	CREATE ROLE contactsa WITH
					LOGIN
					SUPERUSER
					CREATEDB
					CREATEROLE
					INHERIT
					REPLICATION
					CONNECTION LIMIT -1
					PASSWORD 'SplArmonsMAZONTINGEriCi';

					EXCEPTION 
						WHEN DUPLICATE_OBJECT THEN RAISE NOTICE 'not creating role my_role -- it already exists';
END
$$