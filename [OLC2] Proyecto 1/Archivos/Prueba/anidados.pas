program ejemplo;

procedure saludo();
    procedure saludo2();
    begin
        writeln('entro a saludo 2');
    end;

    function saludo3():integer;
    begin
        writeln('entro a saludo 3');
        saludo3 := 10;
        saludo2();
    end;
begin
    writeln('Estoy saludando');
    saludo3();
end;
    
begin
    saludo();
end.

{
    Salida
    Estoy saludando                                                                                             
    entro a saludo 3                                                                                            
    entro a saludo 2
}