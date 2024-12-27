export const formatValueWithRegex = (value) =>{
    var converter = parseFloat(value).toFixed(2);
    converter = converter.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
    return converter;
}

export const formatValueIntWithRegex = (value) =>{
    var converter = parseFloat(value).toFixed(0);
    converter = converter.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
    return converter;
}

export const reload = async e => {

    window.location.reload(false);
   
};