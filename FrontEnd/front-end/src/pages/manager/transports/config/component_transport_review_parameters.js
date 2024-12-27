import React from 'react';
import MenuSuperAdmin from "../../../../components/menus/menu_manager"
import Toolbar from "../../../../components/utils/toolbar_manager"
import Footer from '../../../../components/utils/footer'
import api from '../../../../service/api'

class TransportReviewParameters extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            price: 0.0,
            token: "",
            valueFuel: 0,
            valueHoliday: 0,
            valueSaturday: 0,
            valueSunday: 0,
            valueToll: 0,
            id: "",
            valueSelect: "",
            considerTruckBreakDowns:""
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Truck"

    }

    componentDidMount() {

        const response = api.get("/api/v1/config", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({considerTruckBreakDowns:result.data.considerTruckBreakDowns,valueSelect : result.data.typeAnalysis, id: result.data.id, valueToll: result.data.valueToll, valueSunday: result.data.valueSunday, valueFuel: result.data.valueFuel, valueHoliday: result.data.valueHoliday, valueSaturday: result.data.valueSaturday }));
    }

    handleTruckBreakDown = (e) => {
    
        this.setState({considerTruckBreakDowns: e.target.value });
    
      }

      handleChangeLicense = (e) => {
    
        this.setState({ valueSelect: e.target.value });
    
      }


    handlePost = async e => {
        e.preventDefault();
        let isToPost = true
        const { id, valueToll, valueSunday,valueSelect, valueFuel, valueSaturday, valueHoliday ,considerTruckBreakDowns} = this.state;

        if (isToPost) {
            const response = await api.put("/api/v1/config/" + id, JSON.stringify({
                valueToll: valueToll,
                valueSunday: valueSunday,
                valueFuel: valueFuel,
                valueSaturday: valueSaturday,
                valueHoliday: valueHoliday,
                typeAnalysis: valueSelect,
                considerTruckBreakDowns:considerTruckBreakDowns
            }), {
                headers: {
                    Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });

            if (response.status == 200) {
                alert("Atualizado com sucesso!")
                window.location.reload(false);

            } else {
                this.setState({ error: "Campos Inválidos" });
            }
        }
    }

    render() {

        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuSuperAdmin />
                {/* Content Start */}
                <div className="content">
                    <Toolbar />
                    {/* Form Start */}
                    <div className="container-fluid pt-4 px-4">
                        <div className="row g-4">
                            <div className="col-sm-12 col-xl-12">
                                <div className="bg-light rounded h-100 p-4">
                                    <a className="nav-item nav-link active" style={{ fontSize: '20px' }}><i className="fa fa-cog me-2" />Parametizar modelo de análise</a>
                                    <form>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Valor portagem por kms</label>
                                            <input type="text" className="form-control" required id="text" value={this.state.valueToll} onChange={e => this.setState({ valueToll: e.target.value })} />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Valor extra ao Sábado</label>
                                            <input type="text" className="form-control" required id="text" value={this.state.valueSaturday} onChange={e => this.setState({ valueSaturday: e.target.value })} />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Valor extra ao Domingo</label>
                                            <input type="text" className="form-control" required id="text" value={this.state.valueSunday} onChange={e => this.setState({ valueSunday: e.target.value })} />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Valor extra num feriado</label>
                                            <input type="text" className="form-control" required id="text" value={this.state.valueHoliday} onChange={e => this.setState({ valueHoliday: e.target.value })} />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Preço Combustivel</label>
                                            <input type="text" className="form-control" required id="text" value={this.state.valueFuel} onChange={e => this.setState({ valueFuel: e.target.value })} />
                                        </div>
                                        <div className="form-floating mb-3">
                                            <select onChange={this.handleChangeLicense} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                                                <option selected>{this.state.valueSelect}</option>
                                                <option value={"BOTH"}> BOTH</option>
                                                <option value={"OCCUPANCY_RATE"}>OCCUPANCY_RATE</option>
                                                <option value={"COST"}>COST</option>
                                            
                                            </select>
                                            <label htmlFor="floatingSelect">Tipo de análise </label>
                                        </div>

                                        <div className="form-floating mb-3">
                                            <select onChange={this.handleTruckBreakDown} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                                                <option selected>{this.state.considerTruckBreakDowns}</option>
                                                <option value={"YES"}> Sim</option>
                                                <option value={"NO"}>Não</option>
                                                
                                            </select>
                                            <label htmlFor="floatingSelect">Influência de avarias</label>
                                        </div>
                                        
                                        <button className="btn btn-primary" onClick={this.handlePost} style={{ width: 240 }}>Atualizar parâmetros</button>


                                    </form>

                                </div>
                            </div>

                        </div>
                    </div>
                    {/* Form End */}
                    {/* Footer Start */}
                    <Footer />
                    {/* Footer End */}
                </div>
            </div>
        )
    }
}



export default TransportReviewParameters

