
import PropTypes from 'prop-types';
import TextLeftDeco from './Components/TextDecorationLeft/TextLeftDeco';

const ItemCaroursel = props => {
    const { id, src } = props;

    return (
        <div style={{
            // backgroundImage: `url(${src})`,
            backgroundColor: '#E5DFCA',
            width: '100%',
            height: '700px',
            display: 'flex',
            alignItems: 'center'
        }}>
            <TextLeftDeco/>
        </div>
    )
}

ItemCaroursel.propTypes = {
    id: PropTypes.string,
    src: PropTypes.string,
}

export default ItemCaroursel
