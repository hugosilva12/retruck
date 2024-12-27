import React from 'react';

class MenuManager extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            imagesrc: "",
            photofilename: "anonymous.png",
        }

        this.state.photofilename = "anonymous.png";
        this.state.imagesrc = localStorage.getItem("photopath");
        if (this.state.imagesrc == undefined) {
            this.state.imagesrc = process.env.REACT_APP_PHOTOPATH + this.state.photofilename;
        }
    }


    render() {
        return (
            <div className="sidebar pe-4 pb-3">
                <nav className="navbar bg-light navbar-light">
                    <a href="/homemanager" className="navbar-brand mx-4 mb-3">
                        <h3 className="text-primary"><i className="fa fa-hashtag me-2" />Manager</h3>
                    </a>
                    <div className="d-flex align-items-center ms-4 mb-4">
                        <div className="position-relative">
                            <img className="rounded-circle" src={this.state.imagesrc} alt="" style={{ width: '60px', height: '60px' }} />
                            <div className="bg-success rounded-circle border border-2 border-white position-absolute end-0 bottom-0 p-1" />
                        </div>
                        <div className="ms-3">
                            <h6 className="mb-0" style={{ fontSize: '20px' }}>{localStorage.getItem("name")}</h6>
                            <span>Manager</span>
                        </div>
                    </div>
                    <div className="navbar-nav w-100">
                        <a href="/homemanager" className="nav-item nav-link active" style={{ fontSize: '20px' }}><i className="fa fa-home me-2" />Home</a>
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown" style={{ fontSize: '20px' }}><i className="fa fa-truck me-2" /> Camiões</a>
                            <div className="dropdown-menu bg-transparent border-0">
                                <a href="/addtruck" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-truck">  Adicionar Camião</i> </a>
                                <a href="/listtruck" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-address-card">  Visualizar Camiões </i></a>
                                <a href="/addrevision" className="dropdown-item" style={{ fontSize: '18px' }}><i class="fa fa-truck" >  Adicionar Revisão </i></a>
                                <a href="/listrevision" className="dropdown-item" style={{ fontSize: '18px' }}><i class="fa fa-credit-card" >  Visualizar Revisões </i></a>
                            </div>
                        </div>
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown" style={{ fontSize: '20px' }}><i className="fa fa-book me-2" /> Transportes</a>
                            <div className="dropdown-menu bg-transparent border-0">
                                <a href="/transportpending" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit"> Pendentes</i> </a>
                                <a href="/listtransport" className="dropdown-item"  style={{ fontSize: '18px' }}> <i class="fa fa-truck">  Visualizar Todos </i></a>
                            </div>
                        </div>
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown" style={{ fontSize: '20px' }}><i className="fa fa-sitemap me-2" /> Serviços</a>
                            <div className="dropdown-menu bg-transparent border-0">
                                <a href="/services" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Visualizar Serviços</i> </a>
                            </div>
                        </div>
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown" style={{ fontSize: '20px' }}><i className="fa fa-calendar me-2" /> Ausências</a>
                            <div className="dropdown-menu bg-transparent border-0">
                                <a href="/absencepending" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Ausências Pendentes</i> </a>
                                <a href="/absence" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-calendar">  Visualizar Ausências </i></a>
                            </div>
                        </div>
                        <a href="/config" className="nav-item nav-link" style={{ fontSize: '18px' }}><i className="fa fa-cog me-2" />Configs</a>
                    </div>
                </nav>
            </div>

        )
    }
}


export default MenuManager

