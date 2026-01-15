namespace Directory_Service.Shared;

public static class ConstantsSql
{
    public static string DAPPER_SQL = """
                                      do
                                      $$
                                          begin
                                              if @newPath::ltree <@ @oldPath::ltree then
                                                  raise exception 'Нельзя установить родителем в свое дочернее подразделение';
                                              end if;
                                          end ;
                                      $$;

                                      with moving as (select @oldPath::ltree         as old_path,
                                                             @newPath::ltree         as new_parent_path,
                                                             nlevel(@oldPath::ltree) as old_depth)

                                      update department
                                      set path = (select new_parent_path || subpath(path, old_depth - 1))
                                      from moving
                                      where path <@ old_path
                                        and not new_parent_path <@ old_path;

                                      update department
                                      set depth = nlevel(path) - 1
                                      where path <@ @updateDepthPath::ltree;
                                      """;
}