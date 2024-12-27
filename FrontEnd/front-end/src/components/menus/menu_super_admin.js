import React from 'react';

class MenuSuperAdmin extends React.Component {

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
                    <a href="/" className="navbar-brand mx-4 mb-3">
                        <h3 className="text-primary"><i className="fa fa-hashtag me-2" />SuperAdmin</h3>
                    </a>
                    <div className="d-flex align-items-center ms-4 mb-4">
                        <div className="position-relative">
                            <img className="rounded-circle" src={this.state.imagesrc} alt="" style={{ width: '40px', height: '40px' }} />
                            <div className="bg-success rounded-circle border border-2 border-white position-absolute end-0 bottom-0 p-1" />
                        </div>
                        <div className="ms-3">
                            <h6 className="mb-0" style={{ fontSize: '20px' }}>{localStorage.getItem("name")}</h6>
                            <span>Super-Admin</span>
                        </div>
                    </div>
                    <div className="navbar-nav w-100">
                        <a href="/home" className="nav-item nav-link active" style={{ fontSize: '20px' }}><i className="fa fa-home me-2" />Home</a>
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown" style={{ fontSize: '20px' }}><i className="fa fa-users me-2" /> Utilizadores</a>
                            <div className="dropdown-menu bg-transparent border-0">
                                <a href="/adduser" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Adicionar Utilizadores</i> </a>
                                <a href="/listemployee" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-address-card"  >  Visualizar Funcionários </i></a>
                                <a href="/listcustomer" className="dropdown-item" style={{ fontSize: '18px' }}><i class="fa fa-credit-card">  Visualizar Clientes </i></a>
                            </div>
                        </div>
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown" style={{ fontSize: '20px' }}><i className="fa fa-sitemap me-2" />Organizações </a>
                            <div className="dropdown-menu bg-transparent border-0">
                                <a href="/addorganization" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Adicionar Organização</i> </a>
                                <a href="/listorganization" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-address-card">  Visualizar Organizações </i></a>
                            </div>
                        </div>

                    </div>
                </nav>
            </div>

        )
    }
}


export default MenuSuperAdmin

