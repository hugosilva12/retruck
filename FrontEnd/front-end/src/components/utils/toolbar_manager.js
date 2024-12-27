import React from 'react'
import './navbar.css'
class ToolbarManager extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            imagesrc: "",
            photofilename: "anonymous.png",
            showMenu: false
        }

        this.state.photofilename = "anonymous.png";
        this.state.imagesrc = localStorage.getItem("photopath");
        if (this.state.imagesrc == undefined) {
            this.state.imagesrc = process.env.REACT_APP_PHOTOPATH + this.state.photofilename;
        }
    }
    change = (e) => {
        this.setState({ showMenu: !this.state.showMenu })

    }

    //Logout App
    logOut = (e) => {
        localStorage.setItem("token", "");
        localStorage.setItem("id", "");
        localStorage.setItem("photopath", "");
        localStorage.setItem("name", "")
        window.location.href = '/';
    }

    render() {
        return (
            <>
                <nav className="navbar navbar-expand bg-light navbar-light sticky-top px-4 py-0">
                    {this.state.showMenu == true &&
                        <>
                            <div className="menu">
                                <a href="/homemanager" className="nav-item nav-link active" style={{ fontSize: '20px' }}><i className="fa fa-home me-2" />Home</a>
                                <a href="/addtruck" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-truck">  Adicionar Camião</i> </a>
                                <a href="/listtruck" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-address-card">  Visualizar Camiões </i></a>
                                <a href="/addrevision" className="dropdown-item" style={{ fontSize: '18px' }}><i class="fa fa-truck" >  Adicionar Revisão </i></a>
                                <a href="/listrevision" className="dropdown-item" style={{ fontSize: '18px' }}><i class="fa fa-credit-card" >  Visualizar Revisões </i></a>
                                <a href="/transportpending" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit"> Pendentes</i> </a>
                                <a href="/listtransport" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-truck">  Visualizar Todos </i></a>
                                <a href="/services" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Visualizar Serviços</i> </a>
                                <a href="/absencepending" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Ausências Pendentes</i> </a>
                                <a href="/absence" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-calendar">  Visualizar Ausências </i></a>
                                <a href="/config" className="dropdown-item" style={{ fontSize: '18px' }}><i className="fa fa-cog me-2" />Configs</a>
                            </div>

                        </>
                    }
                    <a href="/homemanager" className="navbar-brand d-flex d-lg-none me-4">
                        <h2 className="text-primary mb-0"><i className="fa fa-hashtag" /></h2>
                    </a>

                    <a onClick={this.change} className="sidebar-toggler flex-shrink-0">
                        <i className="fa fa-bars" />
                    </a>

                    <div className="navbar-nav align-items-center ms-auto">
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown">
                                <img className="rounded-circle me-lg-2" src={this.state.imagesrc} alt="" style={{ width: '40px', height: '40px' }} />
                                <span className="d-none d-lg-inline-flex">{localStorage.getItem("name")}</span>
                            </a>
                            <div className="dropdown-menu dropdown-menu-end bg-light border-0 rounded-0 rounded-bottom m-0">
                                <a onClick={this.logOut} className="dropdown-item">Log Out</a>
                            </div>
                        </div>
                    </div>
                </nav>
            </>
        )
    }
}


export default ToolbarManager

